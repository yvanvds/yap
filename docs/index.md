---
layout: front
menus: Home
---
Yap certainly is not an alternative for MAX or PD, although it has a lot in common with those environments. The main advantage of Yap is that its patches can be read by YSE. YSE is cross-platform and allows DSP programming in C++ by default. But when using YSE from C#, programming DSP is not an option (You don't want to do that in a managed language). Another 'problem' is that you can't add C++ DSP code at runtime. This is where Yap comes in. 

Patches are basically JSON files with objects and connections. A patch defines the DSP and message logic to play audio. 

If you are an audio programmer and you're interesting in the patcher functionality of YSE, you might want to use Yap to create the patches your application needs. Yap also makes it easier to experiment with audio patches. Every Yap object is mirrored by a standard C++ in YSE. Which means that it will be easy to convert your Yap patches to C++ code for more performance. 

## What is YapView?

The GUI part of Yap can be compiled as a separate C# package. It is not linked to YSE directly, but uses an interface where programmers can implement all interaction with the GUI. If you are looking for a visual programming interface, either for audio, video or shader programming, you might want to look at YapView.

