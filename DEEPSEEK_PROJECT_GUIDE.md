# DeepSeek Project Guide – Cognitive Project

## 1) Project Summary
This repository is a mixed cognitive/VR training project with two main directions:

1. Unity-based VR cricket training system
   - Focus: visual attention, reaction time, anticipation, hand-eye coordination, and cognitive training for cricket players.
   - Main idea: simulate cricket bowling and collect performance data in an immersive environment.

2. Python-based cognitive tools
   - Focus: simple interactive cognitive exercises such as reaction testing and memory assistance.
   - Main files: app.py, game_reaction.py, reaction_game.html.

---

## 2) What is already implemented

### A. Unity side
The Unity project contains cricket-related scripts under Assets/:

- Assets/BallThrower.cs
  - Handles ball physics and launch behavior.
  - Computes trajectory based on start position, target position, timing, swing, and spin.
  - Applies velocity and angular velocity to the ball Rigidbody.

- Assets/BowlingManager.cs
  - Controls bowling behavior and ball delivery flow.
  - Supports different bowler types and ball variations.
  - Chooses variation, spawns a ball, and triggers launch via BallThrower.

### B. Python side
- app.py
  - Streamlit app for a cognitive memory assistant.
  - Uses Groq API to answer memory-related questions.
  - Currently contains a hardcoded API key, which should be moved to environment variables.

- game_reaction.py
  - Streamlit reaction-time game.
  - Shows a red-to-green visual cue and records reaction times.
  - Stores scores in session state and shows best/average attempts.

- reaction_game.html
  - Standalone HTML/JavaScript version of the reaction game.
  - Includes score tracking, visuals, and instructions.

---

## 3) Project Architecture at a Glance

### Unity / VR part
- Goal: build a training experience where users interact with a cricket environment and improve visual-cognitive skills.
- Core gameplay loop:
  1. Bowl variation is selected.
  2. Ball is spawned.
  3. Ball trajectory is computed.
  4. Ball is launched toward a target.
  5. Player reacts or interacts with the environment.

### Python / Web part
- Goal: provide simple cognitive tests and assistant-style interactions through web apps.
- Current tools are lightweight and mostly prototype-level.

---

## 4) Strengths of the current project
- Clear concept and strong domain relevance.
- Unity and web components are already separated cleanly.
- The project has a solid foundation for both VR simulation and cognitive testing.
- README explains the scientific and training goals very well.

---

## 5) Important issues / improvement areas

### Security / reliability
- app.py contains a hardcoded API key.
- This should be replaced with a secure environment variable.

### Prototype maturity
- The Unity logic is promising, but the project appears to be in an early or incomplete state.
- The Python tools are good for demos, but they still need better integration and data management.

### Missing pieces for a complete product
- User login/profile management
- Session data storage and analytics
- Better scoring logic and feedback system
- Integration between reaction tests and VR training modules
- Proper UI/UX flow for training sessions

---

## 6) Suggested next steps to build this project

### Phase 1 – MVP
Choose one primary experience and finish it well:
- Option A: Reaction training web app
- Option B: VR cricket training prototype
- Option C: Combined system with both web and VR modules

### Phase 2 – Core features
- Add a database for scores and sessions
- Save player history
- Show charts and trends
- Add difficulty progression

### Phase 3 – Advanced features
- Eye-tracking integration (if hardware available)
- Adaptive difficulty based on performance
- Multiple training modules
- Coach/dashboard features

---

## 7) Recommended implementation plan
1. Stabilize the current web app first.
2. Improve data handling and logging.
3. Refine the Unity bowler/ball interaction loop.
4. Add a clear user flow: start session → train → analyze results.
5. Build a simple dashboard for metrics.

---

## 8) DeepSeek Prompt (ready to use)
Use this prompt with DeepSeek to continue development:

"Analyze this repository and help me build the next version of this cognitive/VR training project. The project includes a Unity-based cricket training system and Python-based cognitive web apps. Focus on the following:
- Understand the current architecture and main files.
- Suggest a clean MVP plan.
- Improve the code structure.
- Help implement one feature at a time.
- Recommend secure handling for the Groq API key.
- Propose how to connect the reaction test and VR training modules into one coherent experience.
- Provide step-by-step implementation guidance with code examples."

---

## 9) Extra guidance for DeepSeek
When asking DeepSeek for help, be specific. Good examples:
- "Help me refactor app.py to use environment variables securely."
- "Help me add score saving to the reaction game."
- "Help me extend BowlingManager to support multiple ball types."
- "Help me create a Unity scene flow for training, results, and dashboard."

---

## 10) Quick developer notes
- This project is ambitious and well-conceived.
- Best approach is to build one module at a time instead of trying to finish everything at once.
- For the first version, focus on a simple but polished experience that works well.
