# app.py - FREE Groq API based Cognitive Memory Assistant

import os
import streamlit as st
from datetime import datetime

try:
    from groq import Groq
except ImportError:
    Groq = None

try:
    from dotenv import load_dotenv
except ImportError:
    load_dotenv = None

load_dotenv = load_dotenv or (lambda: None)
load_dotenv()

GROQ_API_KEY = os.getenv("GROQ_API_KEY", "").strip()

st.set_page_config(page_title="Cognitive Memory Assistant", page_icon="🧠")
st.title("🧠 Cognitive Memory Support System")
st.markdown("*FREE API - Powered by Groq*")

if not GROQ_API_KEY or Groq is None:
    st.error("GROQ_API_KEY is missing or the Groq package is not installed.")
    st.info("Set the GROQ_API_KEY environment variable or create a .env file before running this app.")
    st.stop()

# Groq client
client = Groq(api_key=GROQ_API_KEY)

# Sidebar
with st.sidebar:
    st.success("✅ FREE API Connected")
    st.write("Ask me about:")
    st.write("- 📍 Where you kept something")
    st.write("- 🏥 What doctor said")
    st.write("- ⏰ Reminders")
    
    if st.button("Clear Chat"):
        st.session_state.messages = [{
            "role": "system",
            "content": "You are a memory assistant for elderly. Help remember appointments, where items are kept, conversations. Keep answers short and clear."
        }]
        st.rerun()

# Memory
if "messages" not in st.session_state:
    st.session_state.messages = [
        {"role": "system", "content": "You are a memory assistant for elderly. Help remember appointments, where items are kept, conversations. Keep answers short and clear."}
    ]

# Chat history dikhao
for msg in st.session_state.messages:
    if msg["role"] != "system":
        with st.chat_message(msg["role"]):
            st.write(msg["content"])

# Input lo
user_input = st.chat_input("Kya bhool gaye? Ask me...")

if user_input:
    # User message dikhao
    st.session_state.messages.append({"role": "user", "content": user_input})
    with st.chat_message("user"):
        st.write(user_input)
    
    # AI response
    with st.chat_message("assistant"):
        with st.spinner("🧠 Soch raha hoon..."):
            try:
                response = client.chat.completions.create(
                    model="llama-3.3-70b-versatile",
                    messages=st.session_state.messages,
                    temperature=0.7,
                    max_tokens=500
                )
                reply = response.choices[0].message.content
                st.write(reply)
                st.session_state.messages.append({"role": "assistant", "content": reply})
            except Exception as e:
                st.error(f"Error: {e}")
                st.info("Check API key ya internet connection")