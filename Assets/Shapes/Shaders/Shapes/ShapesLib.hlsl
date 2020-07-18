#if !defined(SHAPES_LIB)
#define SHAPES_LIB

void RayDistanceToPoint_float(in float3 Origin, in float3 Direction, in float3 Point, out float Distance, out float Depth) {
	Depth = dot(Point - Origin, Direction);
	Distance = distance(Origin + Direction * Depth, Point);
}

void RayIntersectPlane_float(in float3 Origin, in float3 Direction, in float3 PointOnPlane, in float3 PlaneNormal, out float3 Intersection) {
    float3 normal = normalize(PlaneNormal);
    float3 nDir = normalize(Direction);
    float distance = -dot(normal, PointOnPlane);
    float a = dot(nDir, normal);
    float num = -dot(Origin, normal) - distance;
    if (abs(a) < 0.001f)
    {
        Intersection = Origin;
    }
    float enter = num / a;
    Intersection = Origin + nDir * enter;
}

void RayIntersectShapeSpace_float(in float3 Origin, in float3 Direction, out float2 Intersection) {
    float3 nDir = normalize(Direction);
    float a = dot(nDir, float3(0, 0, 1));
    float num = -dot(Origin, float3(0, 0, 1));
    if (abs(a) < 0.001f)
    {
        Intersection = Origin;
    }
    float enter = num / a;
    Intersection = (Origin + nDir * enter).xy;
}

void RayDistanceToCircle_float(in float3 Origin, in float3 Direction, in float3 Center, in float3 Normal, in float Radius, out float Distance) {
    float3 intersection;
    RayIntersectPlane_float(Origin, Direction, Center, Normal, intersection);
    float d = distance(intersection, Center);
    Distance = Radius - d;
}

void PointDistanceToLine_float(in float2 Point, in float2 Pos0, in float2 Pos1, out float Distance) {
    float3 intersection;

    float2 dir = Pos1 - Pos0;
    float2 l = length(dir);
    float2 nDir = dir / l;
    float2 pDir = Point - Pos0;
    float c = cross(float3(pDir, 0), float3(nDir, 0)).z;
    float d = dot(pDir, nDir);

    float s0 = step(0, d);
    float s2 = 1 - step(l, d);
    float s1 = s0 * s2;

    float r0 = distance(Point, Pos0);
    float r2 = distance(Point, Pos1);
    float r1 = lerp(1, abs(c), s1);

    Distance = min(min(r0, r2), r1);
}

void RayDistanceToLine_float(in float3 Origin, in float3 Direction, in float3 Point0, in float3 Point1, out float Distance, out float Depth) {
    float3 p = Origin + Direction * 1000;
    float3 v = Point1 - Point0;
    float3 vector2 = p - Origin;
    float3 rhs = Point0 - Origin; 
    float num = dot(v, v);
    float num2 = dot(vector2, vector2);
    float num3 = dot(vector2, rhs);
    float num5 = dot(v, rhs);
    float num6 = dot(v, vector2);
    float num7 = num * num2 - num6 * num6;
    float s = saturate((num6 * num3 - num5 * num2) / num7);
    float num4 = (num6 * s + num3) / num2;
    if (num4 < 0)
    {
        num4 = 0;
        s = saturate(-num5 / num);
    }
    else
    {
        if (num4 > 1)
        {
            num4 = 1;
            s = saturate((num6 - num5) / num);
        }
    }
    float3 vector3 = Point0 + v * s;
    float3 k = vector2 * num4;
    float3 vector4 = Origin + k;
    Distance = length(vector3 - vector4);
    Depth = length(k);
}

float3 ObjectToViewPos(in float3 os) {

    //return mul(GetWorldToHClipMatrix(), mul(GetObjectToWorldMatrix(), float4(positionOS, 1.0)));
    return mul(GetWorldToHClipMatrix(), float4(os, 1));
    //float3 r = TransformWorldToView(TransformObjectToWorld(os.xyz));
    //r.xy = r.xy / r.z;
    //r.z -= 0.5f;
    //return r;
    //return mul(UNITY_MATRIX_MV, float4(os, 1)); 
}

float4x4 objectToClip;
float4x4 clipToObject;

void GetMinMaxVS_float(in float3 MinOS, in float3 MaxOS, out float3 MinVS, out float3 MaxVS) {
    //float4x4 objectToClip = mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix());
    //float4x4 clipToObject = transpose(objectToClip);
    /*float4 minCS = mul(objectToClip, float4(MinOS, 1));
    float4 maxCS = mul(objectToClip, float4(MaxOS, 1));
    minCS.z = 0;
    maxCS.z = 0;
    minCS.w = 1;
    maxCS.w = 1;
    float4 minR = min(minCS, maxCS);
    float4 maxR = max(minCS, maxCS);
    MinVS = mul(clipToObject, minR);
    MaxVS = mul(clipToObject, maxR);*/

    float3 size = MaxOS - MinOS;
    float3 minX = mul(objectToClip, float4(MinOS.x + size.x, MinOS.y, MinOS.z, 1));
    float3 minY = mul(objectToClip, float4(MinOS.x, MinOS.y + size.y, MinOS.z, 1));
    float3 minZ = mul(objectToClip, float4(MinOS.x, MinOS.y, MinOS.z + size.z, 1));
    float3 maxX = mul(objectToClip, float4(MaxOS.x - size.x, MaxOS.y, MaxOS.z, 1));
    float3 maxY = mul(objectToClip, float4(MaxOS.x, MaxOS.y - size.y, MaxOS.z, 1));
    float3 maxZ = mul(objectToClip, float4(MaxOS.x, MaxOS.y, MaxOS.z - size.z, 1));
    float4 minCS = mul(objectToClip, float4(MinOS, 1));
    float4 maxCS = mul(objectToClip, float4(MaxOS, 1));
    
    float2 minR = min(min(min(MinOS.xy, minX.xy), minY.xy), minZ.xy);
    minR = min(min(min(MaxOS.xy, maxX.xy), maxY.xy), maxZ.xy);
    float2 maxR = max(max(max(MinOS.xy, minX.xy), minY.xy), minZ.xy);
    maxR = max(max(max(MaxOS.xy, maxX.xy), maxY.xy), maxZ.xy);

    MinVS = mul(clipToObject, float4(minR, 1, 1));
    MaxVS = mul(clipToObject, float4(maxR, 1, 1));
    //MinVS = min(min(min(MinVS, minX), minY), minZ);
    //MinVS = min(min(min(MaxVS, maxX), maxY), maxZ);
    //MaxVS = max(max(max(MinVS, minX), minY), minZ);
    //MaxVS = max(max(max(MaxVS, maxX), maxY), maxZ);
}

#endif