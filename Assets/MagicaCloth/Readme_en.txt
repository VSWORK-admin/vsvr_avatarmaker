//------------------------------------------------------------------------------
// Magica Cloth
// Copyright (c) Magica Soft, 2020
// https://magicasoft.jp
//------------------------------------------------------------------------------

### About
Magica Cloth is a high-speed cloth simulation operated by Unity Job System + Burst compiler.


### Support Unity versions
Unity2018.4.0(LTS) or higher


### Feature

* Fast cloth simulation with Unity Job System + Burst compiler
* Works on all platforms except WebGL
* Implement BoneCloth driven by Bone (Transform) and MeshCloth driven by mesh
* MeshCloth can also work with skinning mesh
* Easy setup with an intuitive interface
* Time operation such as slow is possible
* With full source code


### Documentation
Since it is an online manual, please refer to the following URL for details.
https://magicasoft.jp/magica-cloth


### Release Notes
[v1.6.1]
Improvement: Improved the friction processing algorithm. The problem of particles vibrating has been reduced.
Fix: Fixed a problem that vertex painting could not be performed properly when there are two or more inspector windows in the editor.
Fix: Fixed an issue that caused an error at the end of execution in the editor.

[v1.6.0]
Improvement: Improved the behavior of ClampPosition / ClampRotation. Collision detection has priority over movement restriction.
Improvement: Improved collision determination processing.
Improvement: Improved the rotation line generation algorithm.
Improvement: Collider Gizmo is basically hidden when not selected.
Improvement: Added a reset simulation button to the running inspector.
Improvement: Improved the friction processing algorithm.
Improvement: Enabled to specify the maximum number of connections in [Near Point] of Restore Distance.
Improvement: Changed the Virtual Deformer weight calculation method to the average weight value of the referenced skinning mesh vertices.
This greatly reduces the problem of unintended vertex deformation during animation.
Improvement: Fixed vertices in VirtualDeformer were set to be completely excluded from the calculation in some situations.
This greatly reduces the problem of unintended vertex deformation during animation.
Improvement: Renamed Adjust Line to Rotation Interpolation.
Improvement: Added FixedNonRotation flag to Rotation Interpolation.
If this flag is set to ON, fixed particles will not rotate at all.
Fix: Fixed an issue where Global Collider was not working properly.

[v1.5.1]
Feature: Added API for accessing [Distance Disable] parameter.
Feature: Added API for accessing [External Force] parameter.
Improvement: Connection control between components has been strengthened.
Fix: Fixed an issue where an error would occur if [Distance Disable] was turned On / Off during execution.
Fix: Fixed an issue that caused an error when creating a cloth component with LateUpdate during delayed execution.
Fix: Changed the delayed execution to be executed at PostLateUpdate instead of at the end of LateUpdate.
Fix: Fixed an issue where inactive render deformers were being calculated.
Fix: Fixed an issue where inactive render deformers were causing a memory leak.
Fix: Fixed [RenderMeshVertexUsed] [VirtualMeshVertexUsed] values on cloth monitor to be correct.

[v1.5.0]
Feature: Added delayed execution mode.
Improvement: Improved performance.
Improvement: Displaying write time to mesh in profiler.
Improvement: Render deformer normal / tangent recalculation can now select normal only or normal + tangent.
Improvement: Scene view can be rotated by Alt + mouse drag while vertex painting.
Fix: Fixed incorrect scale calculation when writing to bones.
Fix: Fixed an issue where references to parent bones could be lost when referring to one bone multiple times.
Fix: Fixed an issue where teleport might not work properly.
Fix: Fixed an issue when selecting the wind component when the cloth monitor was hidden.
Fix: Fixed an issue where data was not written correctly when editing in prefab mode.
Fix: Modified to redraw the scene view when editing MeshSpring axes in the inspector.
Note: The update mode [Once Per Frame] will be deprecated in the future.

[v1.4.2]
Improvement: When creating a collider, it has been changed to adjust the collider scale from the parent scale.
Fix: Fixed a problem where mesh was broken when SkinnedMeshRenderer and MeshRenderer were mixed using Unity2018.
Fix: Fixed Capsule Collider gizmo not displaying correctly.
Fix: Fixed an issue where cloth simulation was not running on frames with cloth components attached.

[v1.4.0]
Feature: Added dress-up system (Avatar, AvatarParts).
Feature: Teleport is turned off by default.
Improvement: Reduced vibration caused by movement.
Improvement: When creating a cloth component object, it is set to inherit the parent name.
Fix: Fixed issue where MeshOptimizeMissmatch error would occur when loading from asset bundle.
Fix: Fixed an issue where the scene view was not redrawn when painting vertices.
Fix: Fixed an issue where writing transforms was not correct when adding / removing cloth components repeatedly.
Fix: Fixed collider to correctly reflect transform scale.
Fix: Fixed an issue where an error would occur if the main camera did not exist.
Fix: Fixed an issue where data was not created when attaching a RenderDeformer with multiple renderers selected.

[v1.3.0]
Feature: Added wind function (Wind).
Feature: Added wind sample scene (WindSample).
Improvement: Changed cloth team preprocessing from C # to JobSystem.

[v1.2.0]
Feature: Added blending function with original posture (Blend Weight).
Feature: Added the function to disable simulation by distance (Distance Disable).
Feature: Added a sample scene for distance disable function (DistanceDisableSample).
Improvement: Added scrollbar to cloth monitor.
Improvement: Data can be created even if the mesh has no UV value.
Improvement: Enhanced error handling.
Fix: Fixed slow playback bug. Time.timeScale works correctly.
Fix: Fixed an issue where an error occurred when duplicating a prefab with [Ctrl+D].
Fix: Fixed an issue where trying to create data without vertex painting would result in an error.

[v1.1.0]
Feature: Added support for Unity2018.4 (LTS).
Improvement: Error details are now displayed along with error codes.
Improvement: Vertex paint now records by vertex hash instead of vertex index.
Fix: If two or more MagicaPhysicsManagers are found, delete those found later.

[v1.0.3]
Fix: Fixed the problem that reference to data is lost while editing in Unity2019.3.0.

[v1.0.2]
Fix: Fixed an issue where an error occurred when running in the Mac editor environment.

[v1.0.1]
Fix: Fixed an error when writing a prefab in Unity2019.3.

[v1.0.0]
Note: first release.




