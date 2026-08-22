# game_reaction.py - JavaScript Working Version
import streamlit as st
import random
import time
import pandas as pd
from streamlit.components.v1 import html

st.set_page_config(page_title="Reaction Test Game", page_icon="⚡")

st.title("⚡ Reaction Test Game")
st.write("Click FAST when background turns **GREEN**!")

# Initialize session state
if "scores" not in st.session_state:
    st.session_state.scores = []
if "game_active" not in st.session_state:
    st.session_state.game_active = False
if "reaction_time" not in st.session_state:
    st.session_state.reaction_time = None

# Show stats
col1, col2, col3 = st.columns(3)
if st.session_state.scores:
    col1.metric("Best", f"{min(st.session_state.scores):.3f}s")
    col2.metric("Average", f"{sum(st.session_state.scores)/len(st.session_state.scores):.3f}s")
    col3.metric("Attempts", len(st.session_state.scores))
else:
    col1.metric("Best", "-")
    col2.metric("Average", "-")
    col3.metric("Attempts", "0")

# JavaScript game component
game_html = """
<div id="game-container" style="text-align: center; margin: 20px 0;">
    <div id="color-box" style="
        width: 100%;
        height: 300px;
        background-color: gray;
        border-radius: 20px;
        display: flex;
        align-items: center;
        justify-content: center;
        cursor: pointer;
        transition: all 0.1s ease;
        margin-bottom: 20px;
    ">
        <h2 id="box-text" style="color: white;">⚡ READY</h2>
    </div>
    <button id="start-btn" style="
        padding: 15px 30px;
        font-size: 18px;
        background-color: #4CAF50;
        color: white;
        border: none;
        border-radius: 10px;
        cursor: pointer;
        margin: 10px;
    ">🎮 START GAME</button>
    <button id="click-btn" style="
        padding: 15px 30px;
        font-size: 18px;
        background-color: #2196F3;
        color: white;
        border: none;
        border-radius: 10px;
        cursor: pointer;
        margin: 10px;
        display: none;
    ">🟢 CLICK HERE (ON GREEN)</button>
</div>

<script>
    let gameActive = false;
    let waitingForGreen = false;
    let greenTime = null;
    let timeoutId = null;
    
    const colorBox = document.getElementById('color-box');
    const boxText = document.getElementById('box-text');
    const startBtn = document.getElementById('start-btn');
    const clickBtn = document.getElementById('click-btn');
    
    function setRed() {
        colorBox.style.backgroundColor = '#ff4444';
        boxText.innerHTML = '🔴 WAIT...';
        boxText.style.color = 'white';
        waitingForGreen = true;
        
        // Random delay between 1-4 seconds
        const delay = Math.random() * 3000 + 1000;
        timeoutId = setTimeout(() => {
            setGreen();
        }, delay);
    }
    
    function setGreen() {
        if (!waitingForGreen) return;
        colorBox.style.backgroundColor = '#44ff44';
        boxText.innerHTML = '🟢 CLICK NOW!';
        boxText.style.color = 'black';
        waitingForGreen = false;
        gameActive = true;
        greenTime = Date.now();
        clickBtn.style.display = 'inline-block';
        startBtn.style.display = 'none';
    }
    
    function reset() {
        if (timeoutId) clearTimeout(timeoutId);
        gameActive = false;
        waitingForGreen = false;
        colorBox.style.backgroundColor = 'gray';
        boxText.innerHTML = '⚡ READY';
        clickBtn.style.display = 'none';
        startBtn.style.display = 'inline-block';
    }
    
    startBtn.onclick = () => {
        reset();
        setRed();
    };
    
    clickBtn.onclick = () => {
        if (gameActive && greenTime) {
            const reactionTime = (Date.now() - greenTime) / 1000;
            reset();
            
            // Send result to Streamlit
            const data = { reaction_time: reactionTime };
            const script = document.createElement('script');
            script.innerHTML = `
                const input = document.createElement('input');
                input.type = 'text';
                input.name = 'reaction_time';
                input.value = '${reactionTime}';
                document.body.appendChild(input);
                input.dispatchEvent(new Event('change', { bubbles: true }));
                input.remove();
            `;
            document.body.appendChild(script);
            script.remove();
            
            alert(`✅ Your reaction time: ${reactionTime.toFixed(3)} seconds!`);
        } else if (!gameActive && waitingForGreen) {
            alert('❌ Too early! Wait for GREEN!');
            reset();
        } else {
            alert('❌ Click START GAME first!');
        }
    };
</script>
"""

# Display the game
html(game_html, height=500)

# Handle reaction time from JavaScript
reaction_time = st.text_input("reaction_time", key="reaction_input", label_visibility="collapsed")
if reaction_time:
    try:
        rt = float(reaction_time)
        st.session_state.scores.append(rt)
        st.rerun()
    except:
        pass

# Score history
if st.session_state.scores:
    st.subheader("📊 Score History")
    df = pd.DataFrame({
        "Attempt": range(1, len(st.session_state.scores) + 1),
        "Reaction Time (seconds)": [round(s, 3) for s in st.session_state.scores]
    })
    st.dataframe(df, use_container_width=True)
    
    if st.button("🔄 Reset All Scores"):
        st.session_state.scores = []
        st.rerun()

with st.expander("📖 How to Play"):
    st.write("""
    1. Click **START GAME**
    2. Box turns **RED** - DO NOT CLICK!
    3. After random time (1-4 seconds), box turns **GREEN**
    4. Click **CLICK HERE** button as FAST as possible
    5. Shorter time = Better score!
    """)