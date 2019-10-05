---
layout: page
title: Documentation
menu: true
permalink: /docs/
---

The integrated help function of Yap contains a lot of examples. Be sure to browse through them before you asking any questions. If you do need help, the Issues tab on Github is there for you.

## Data Types
Yap supports these data types:
* **integer**, with its gui object `.i`
* **float**, with its gui object `.f`
* **trigger** or **bang**, with its gui object `.b`
* **message** or **string**, with its gui object `.m`
* **list** *(every message with spaces in it)*
* **audio buffer** *(for DSP objects)*

## Objects
These objects are supported in yap:
* **Math Operations**: `.+`  `.-`  `.*`  `./`
* **DSP Math**: `~+`  `~-`  `~*` `~*`
* **Gui**: `.b` *(button)* `.t` *(toggle)* `.m` *(message)* `.slider` `.text`
* **Flow**: `.s` *(send)* `.r` *(recieve)* `.gate` `.switch` `.route`
* **MIDI**: `.midiout` `.noteon` `.noteoff` `.controlchange` `.polypressure` `.channelpressure` `.programchange`
* **Conversion**: `.mtof` *(midi to frequency)* `.ftom` *(frequency to midi)*
* **DSP Generators**: `~sine` `~saw` `~noise`
* **DSP Filters**: `~lp` *(lowpass)* `~hp` *(highpass)* `~bp` *(bandpass)* `~vcf` 
* **Other DSP**: `~clip` `~line`

## Shortcuts
* `CTRL+E`: Toggle between edit and performance mode
* `CTRL+A`: Add new object
* `t`: Add Toggle
* `b`: Add Button (Bang)
* `i`: Add Integer
* `f`: Add Float

Objects and connections can be deleted by selecting them and pressing `backspace`.
