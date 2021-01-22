#version 450
out vec4 frag_color;

in vec2 fragPosition;

uniform vec4 borderSize;

uniform mat3 transform;

uniform vec4 backgroundColor;
uniform vec4 borderColor;

bool isBorder(vec2 minPos, vec2 maxPos);

void main() {
    vec3 minPos = transform * vec3(0.0f, 1.0f, 1.0f);
    vec3 maxPos = transform * vec3(1.0f, 0.0f, 1.0f);

    if (isBorder(minPos.xy, maxPos.xy)) {
        frag_color = borderColor;
    }
    else {
        frag_color = backgroundColor;
    }
}

bool isBorder(vec2 minPos, vec2 maxPos) {
    return (fragPosition.y <= maxPos.y && fragPosition.y >= maxPos.y - borderSize.x)
        || (fragPosition.x <= maxPos.x && fragPosition.x >= maxPos.x - borderSize.y)
        || (fragPosition.y >= minPos.y && fragPosition.y <= minPos.y + borderSize.z)
        || (fragPosition.x >= minPos.x && fragPosition.x <= minPos.x + borderSize.w);
}
