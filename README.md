# AnimationEaser

This tool allows you to change pace and/or duration of an animation. Can be used to stylize an animation e.g. to make it more toony.



https://user-images.githubusercontent.com/56534400/209669911-07dd19c9-54f9-4eb4-9a62-0a19f4fe0f75.mp4


# Documentation

## Installation

Download and import **AnimationEaser.unitypackage** in Unity Editor.

## Tutorial

1. Create an empty GameObject.

2. Add AnimationEaser script to it.

3. Add an .anim clip to the Original Clip property.

4. Fill in a path/name for a new clip and adjust properties.

5. Press Ease Animation button in the Inspector.

## Properties

- Original Clip: a source clip that will be used to create a new clip

- Easing Workflow: select between Time Dependent and Normalized workflows. Depending on the choice, different properties will be available:

  - Animation Easing Time Dependent: a curve that determines the pacing of the eased clip. The horizontal axis represents time in the new clip, and vertical axis represents time in the original one. The duration of the new clip is determined from the last keyframe in this curve.

  - Animation Easing Time Normalized: a curve that determines the pacing of the eased clip. The horizontal axis represents normalized time in the new clip, and vertical axis represents normalized time in the original one.
  
- Duration Modifier: the clip duration multiplier in the Normalized workflow.

- Clip path: the path to the folder that will contain the new clip. Note: the folder must exist before executing the script.

- Clip name: the name of the new clip.

- Sample Delta Time: a time difference between samples from the original clip. A smaller value means more precision, while a greater value means less memory. 
