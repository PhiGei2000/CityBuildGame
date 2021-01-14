#version 400
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoord;

out struct {
    vec3 normal;
    vec2 texCoord;
} VertexShaderOut;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main() {
    VertexShaderOut.normal = aNormal;
    VertexShaderOut.texCoord = aTexCoord;

    gl_Position = projection * view * model * vec4(aPosition, 1.0f);
}