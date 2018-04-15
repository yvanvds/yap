---
layout: page
title: About
menu: true
permalink: /about/
---

Yap does not aim to be as good as Pure Data or Max/MSP. Instead, it was created specifically to address a few gaps in my current workflow.

* I am working on a sound engine (YSE), with DSP capabilities. Although the engine provides a lot of classes for DSP development, this was not enough. Written in C++, the engine is very capable of fast DSP processing. But recompiling the code is needed for every change. When prototyping, this is not eficient. I ended up adding a patcher system to YSE, but it needed an interface and this is the result.

* YSE also has a C# wrapper. But writing DSP code in a managed environment is a no-go. Again, a patcher system was the obvious answer for that.

* I am working on an interactive environment in C# (Interact). One of the features I intend to include is sending DSP audio instructions directly to smartphones on a WIFI network. LibPD was a possibility here, but then I could not use my own sound engine which also includes other features i would like to use. It would also mean that patches should be created in Pure Data and not in the Interact environment i was working on. That is undesirable because quick changes to the patch and instant reuploading to the mobile devices would not be possible.

* I found that a visual programming environment is not readily available as a nuGet package. So I set out to create the main yap interface as a view (based on SkiaSharp) and make it publically available. The package can also be used with other audio and video engines, once the callbacks are in place.