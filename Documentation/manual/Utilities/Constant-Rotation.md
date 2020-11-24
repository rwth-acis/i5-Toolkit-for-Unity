# Constant Rotation

## Use Case

The <xref:i5.Toolkit.Core.Utilities.ConstantRotation> component makes an object spin at a constant rate around the Y axis (vertical axis).
This can e.g. be used for turntables, rotating planets or other visual effects.

## Usage

In the Unity editor, add the component *Constant Rotation* to a GameObject.
You can alter the *speed* value which is defined in angles per second.
The object can turn into the other direction by giving it a negative speed value.

To stop the rotation, disable the component by setting its property <xref:UnityEngine.Behaviour.enabled> to `false`.

## Functionality

The rotation speed is independent of the framerate.
Moreover, the component makes sure that the object only has a rotation between 0 and 360 degrees.
If it exeeds 360 degrees, the rotation is reset to a value in the range between 0 and 360 that represents the same orientation of the object to avoid floating point imprecisions.