#!/bin/bash

# WARNING! Requires that you have the Vulkan toolchain and glslc installed.

printf "Compiling HLSL shaders...\n"
for file in $(find . -type f -name "*.hlsl"); do
  printf "Compiling \"%s\"... " "$file"
  
  filename=${file%.*}
  
  hasCompiled=false 
  
  if grep -q " VertexShader" "$file"; then
    printf "Vertex... "
    #glslc -fshader-stage=vertex -fentry-point="VertexShader" -fauto-combined-image-sampler -o "${filename}_vert.spv" "$file"
    dxc -spirv -T vs_6_0 -E "VertexShader" -Fo "${filename}_vert.spv" "$file"
    
    if [ $? -ne 0 ]; then
        exit 1
    fi
    
    hasCompiled=true
  fi
  
  if grep -q " PixelShader" "$file"; then
    printf "Pixel... "
    #glslc -fshader-stage=fragment -fentry-point="PixelShader" -fauto-combined-image-sampler -o "${filename}_frag.spv" "$file"
    dxc -spirv -T ps_6_0 -E "PixelShader" -Fo "${filename}_frag.spv" "$file"
    
    if [ $? -ne 0 ]; then
        exit 1
    fi
    
    hasCompiled=true
  fi
  
  if ! $hasCompiled; then
    printf "Ignoring as this file doesn't contain an entry point.\n"
  else
    printf "Done!\n"
  fi
done

# As of 24/06/23, CompileShaders no longer supports GLSL.