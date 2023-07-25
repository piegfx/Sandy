# Sandy Framework
The Sandy Framework is a modern application framework and render engine, designed for applications requiring advanced 
2D and 3D rendering.

**NOTE:** Sandy is currently early in development. Originally designed for the Sandcastle game engine, we have decided to make
the render engine part a separate project, and added a small application framework, so you can just get on and make stuff.

## Q&A

### Is Sandy a game engine?
No. It is not a game engine. It does not provide any entity frameworks, physics engines, etc. It simply provides an
application loop and a render engine.

### How does this compare to an XNA-derived framework?
With the exception of the `SpriteRenderer`, Sandy is not styled to be like XNA whatsoever. It is a modern rendering framework,
and as such does involve a bit more than just loading a model and rendering it. However, Sandy is designed to be as intuitive
as possible for the developer, while still remaining a powerful and modern framework.

### What's planned for the future?
Various things!

* Continuous improvements to the 3D renderer, such as full realtime reflections & per-light shadows.
* A full 2D sprite system, complete with lighting
* Better effect handling (aka, actually being able to use custom shaders, as currently you can't)
* Full SDF shape rendering
* Instancing + batching renderers.
* Terrain

Just to name a few... Sandy is meant to be a powerful modern render engine. And, while still in early stages of development,
we have high hopes and big plans for the future!

### How can I contribute?
To be transparent, currently, we are relatively strict on PRs. Sandy is also meant to be a learning project, and as such
a lot of PRs will be declined. However, PRs that fix bugs or implement smaller features will probably be accepted. If you are
unsure, feel free to open an issue!

And on that note, issues are always welcome.

### Why an LGPL license?
Currently, this is the best license that suits us. Please note, **you are always welcome to use Sandy for your project
without needing to open-source your project!**