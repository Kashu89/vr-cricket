# VR-Based Oculomotor & Visuomotor Training System for Cricket Players

An advanced Virtual Reality (VR) assessment and training platform engineered to quantify, analyze, and enhance the visual, cognitive, and motor skills of cricket players. By combining immersive Unity-based 3D environments with high-fidelity tracking hardware, the system replicates elite-level match conditions to train spatial anticipation, gaze mechanics, peripheral awareness, and reactive motor coupling.

---

## 📌 Project Overview

In cricket, elite performance is highly dependent on specialized visual and motor behaviors. While basic visual traits (e.g., static visual acuity) are similar across player levels, elite batters are distinguished by superior gaze-tracking strategies, early kinematic cue recognition, and highly coordinated visuomotor coupling.

This system serves a dual purpose:
1. **Assessment Tool:** Baseline and monitor player metrics including saccadic velocity, fixation stability, visual reaction time, and anticipation accuracy under variable pressure and anxiety conditions.
2. **Training Tool:** Accelerate skill acquisition using an **Adaptive Difficulty System** and interactive training modules driven by real-time performance analytics.

---

## 🛠️ System Architecture & Workflow

The software architecture follows a modular, pipeline-driven flow designed for low-latency feedback loops and accurate data logging within VR environments.

```
+------------------+     +-----------------------+     +-----------------------+
|  1. User Login   | --> | 2. Module Selection   | --> | 3. VR Simulation      |
|  Profile Load &  |     | Choose from 13 Target |     | Unity Environment    |
|  Historical Data |     | Training Disciplines  |     | Asset Generation      |
+------------------+     +-----------------------+     +-----------+-----------+
                                                                   |
                                                                   v
+------------------+     +-----------------------+     +-----------------------+
|  6. Dashboard    | <-- | 5. Analytics Engine   | <-- | 4. Tracking & Capture |
| Data Persistence |     | Calculate Latency,    |     | Head, Controller, and |
| Visual Reporting |     | Accuracy, & Gaze Index|     | Eye Tracking Pipelines|
+------------------+     +-----------------------+     +-----------------------+
```

1. **User Login & Profile Management:** Restores historical player baselines, user configurations, and custom preferences.
2. **Module Selection:** Provides access to specialized diagnostic and training protocols.
3. **VR Simulation Engine:** Loads immersive stadium or laboratory environments with dynamically managed pitch characteristics, variable lighting conditions, and adaptive distractions.
4. **Data Capture & Tracking Infrastructure:** Aggregates real-time data from multi-modal inputs:
   * **Head Tracking:** Positional and rotational data via 6-DoF HMD sensors.
   * **Controller/Hand Tracking:** Captures bat trajectory, swing mechanics, and contact timing.
   * **Eye Tracking (Binocular):** Monitors fixation points, saccades, smooth pursuits, and gaze anchoring at high sampling rates.
5. **Performance Analysis Engine:** Evaluates visual and physical response latencies, cognitive accuracy, and gaze path optimization.
6. **Analytics & Analytics Dashboard:** Saves performance profiles and provides data visualizations (e.g., accuracy trends, reaction time histograms).

---

## 🧠 Training & Assessment Modules

The system implements a comprehensive suite of 13 targeted modules designed to isolate and build specific components of visual and motor performance.

### 1. Reaction Time & Anticipation Module
* **Purpose:** Train rapid visual reaction times and early prediction of object trajectories (e.g., a bowler’s delivery).
* **Mechanics:** A virtual bowler delivers various cricket ball variations (full delivery, short ball, bouncer) with distinct speeds, spins, and trajectories. Players must predict the delivery type before the ball crosses the pitch's halfway point.
* **Metrics:** Reaction time, anticipation accuracy percentage, and response delay window.

### 2. Gaze Training / Visual Focus Module
* **Purpose:** Refine visual focus patterns, minimize visual drift, and emulate elite-level gaze strategies.
* **Mechanics:** Tracks active gaze locations in real time. Players follow dynamic target elements or maintain fixation during high-distraction scenarios. Provides real-time visual alignment prompts or raycasts for immediate coaching feedback.
* **Metrics:** Fixation duration, gaze stability index, saccade frequency, and visual distraction recovery time.

### 3. Hand–Eye Coordination Module
* **Purpose:** Synchronize cognitive visual perception with physical motor actions.
* **Mechanics:** Integrates controller or hand-tracking frameworks to detect precise target interception. Players perform physical blocks, catches, or precise strikes against rapidly moving targets.
* **Metrics:** Interception accuracy, motor response offset (milliseconds), spatial target deviations, and track alignment scores.

### 4. Peripheral Awareness Module
* **Purpose:** Broaden the operational field of view and detect off-axis athletic movements or field adjustments.
* **Mechanics:** While executing a primary central visual tracking task, secondary targets or situational cues spawn dynamically within the user's peripheral field. Players must acknowledge or react to these peripheral elements without losing tracking of the central vector.
* **Metrics:** Peripheral response latency, detection threshold angle, and multi-task performance attenuation.

### 5. Smooth Pursuit Module
* **Purpose:** Enhance the visual system's capacity to continuously follow moving objects smoothly without relying on corrective saccades.
* **Mechanics:** Simulates realistic ball flights across a wide range of speeds and angular trajectories, requiring continuous tracking.
* **Metrics:** Pursuit gain (ratio of eye velocity to target velocity) and frequency of corrective saccadic jumps.

### 6. Saccadic Movement Module
* **Purpose:** Improve rapid eye movement accuracy when shifting focus between discrete, distant spatial targets.
* **Mechanics:** Prompts rapid focal shifts between alternating targets (e.g., from bowler release point to a fielder positioning cue).
* **Metrics:** Saccadic velocity, target landing accuracy, and overshoot/undershoot intervals.

### 7. Vergence Module
* **Purpose:** Develop optimal eye tracking depth adjustment as an object approaches or moves away rapidly.
* **Mechanics:** Focuses on tracking balls traveling directly along the player's primary depth axis, isolating inward (convergence) and outward (divergence) eye movements.
* **Metrics:** Depth alignment accuracy, binocular fusion tracking latency, and near-point accommodation scores.

### 8. Visual Acuity Module (Dynamic & Static)
* **Purpose:** Diagnose and track clarity of vision under static conditions and rapid motion profiles.
* **Mechanics:** Displays moving or fixed optotypes and high-contrast cricket-specific markers within the VR environment.
* **Metrics:** Visual baseline accuracy across varying speeds and contrast levels.

### 9. Contrast Sensitivity Module
* **Purpose:** Fine-tune target detection thresholds in challenging or variable ambient environments.
* **Mechanics:** Varies ball textures, colors, and illumination levels relative to the stadium backdrop (e.g., white balls against low-lit stands or red balls under changing artificial light).
* **Metrics:** Minimum contrast detection index across diverse color spaces.

### 10. Depth Perception Module
* **Purpose:** Train precise spatial distance calculations and incoming velocity estimations.
* **Mechanics:** Simulates complex ball-flight paths requiring fast judgments of spatial depth, arrival vectors, and bounce coordinates.
* **Metrics:** Stereo-depth accuracy index and spatial margin of error.

### 11. Spatial Awareness Module
* **Purpose:** Maintain structural positioning and awareness of surrounding environment relative to dynamic objects.
* **Mechanics:** Requires the player to execute motor actions while managing spatial positions relative to the pitch crease, stumps, and shifting fielding arrangements.
* **Metrics:** Positional orientation index and boundary proximity awareness.

### 12. Predictive Timing Module
* **Purpose:** Develop acute temporal accuracy when intercepting moving objects at optimal strike points.
* **Mechanics:** Players perform predictive trajectory calculation tasks, such as intercepting an obscured or highly accelerated ball vector at the precise moment of contact.
* **Metrics:** Temporal prediction offset (timing error in milliseconds) and execution synchronization score.

### 13. Visual Memory & Decision-Making Module
* **Purpose:** Strengthen cognitive recall of play sequences and reduce decision-making latency under competitive pressure.
* **Mechanics:** Displays rapid play sequences followed by immediate multi-choice tactical decisions or physical motor choices within micro-second windows.
* **Metrics:** Memory recall accuracy and actionable decision-making speed.

---

## 📈 Data, Analytics & Adaptivity

### Data Persistence Architecture
The system supports scalable data logging paradigms to guarantee performance tracing across extensive training blocks. Developers can integrate the data layer using the following configurations:
* **SQLite:** For localized, low-latency relational data storage directly on standalone devices.
* **Firebase Realtime Database / Firestore:** For cloud synchronization, multi-device access, and unified coach dashboards.
* **JSON Storage:** Standardized file serialization for easy export and ingestion into external mathematical toolchains.

### Interactive Performance Dashboard
The data processing pipeline automatically outputs analytical insights:
* **Reaction Metrics:** Detailed response time distribution graphs and latency tracking.
* **Trend Analysis:** Progressive accuracy curves across historical sessions.
* **Visuomotor Calibration:** Graphical maps comparing ideal target interception coordinates against actual real-world values.

### Closed-Loop Adaptive Difficulty System
To maximize neurological training benefits, the environment dynamically mutates training variables based on multi-session performance thresholds:
* **Velocity Scaling:** Increases or decreases ball/target velocities dynamically.
* **Visual Distractions:** Introduces simulated stadium noise, spectator movement, and erratic weather effects.
* **Target Calibration:** Scales target collision envelopes and shortens target exposure durations.

---

## 💻 Technical Stack & Environment Requirements

* **Game Engine:** Unity (Recommended version: 2022.3 LTS or higher)
* **Programming Language:** C# (.NET Standard 2.1)
* **Hardware Compatibility:** Meta Quest Series (Quest 2, Quest 3, Quest Pro)
* **Input Architecture:** OpenXR Plugin / Oculus XR Integration Framework
* **Eye-Tracking Requirements (Optional Modules):** Extended hardware support via Vive Sense SDK / Meta Movement SDK for headsets with integrated eye-tracking arrays (e.g., Meta Quest Pro, HTC Vive Pro Eye).

---

## 📚 Scientific Grounding & Literature References

The architectural design, metric selections, and behavioral modules implemented in this system are grounded in validated sports science, optometric, and virtual reality research:

1. **Cricket-Specific Vision & Gaze Patterns:** Grounded in reviews mapping oculomotor behavior differences between elite and sub-elite batters (*Fogt & Fogt, "Studies of Vision in Cricket"*) and professional athletic profiling (*Murray et al., "Oculomotor Behavior Predicts Professional Cricket Batting and Bowling Performance"*).
2. **Temporal Occlusion & Anticipation:** Implements training designs validated by sports science meta-analyses for accelerated visual anticipation (*Müller et al., "Accelerating Visual Anticipation in Sport Through Temporal Occlusion Training"*).
3. **VR Validation & Psychological State:** Built using experimental parameters studying athletic performance under environmental stress, performance anxiety, and immersion constraints (*Kelly et al., "Using a virtual reality cricket simulator to explore the effects of pressure, competition anxiety on batting performance"*).
4. **Eye-Tracking Data Quality:** Core spatial tracking logic, precision boundaries, and calibration profiles conform to established clinical validation benchmarks for commercial HMD integrated trackers (*Schuetz & Fiehler, "Eye Tracking in Virtual Reality: Vive Pro Eye Spatial Accuracy, Precision, and Calibration Reliability"*).
# cricket-vr
# cricket-vr
