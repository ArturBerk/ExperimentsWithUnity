#include "ShapesLib.hlsl"

#ifdef SHADER_API_D3D11
// DX11-specific code, e.g.
StructuredBuffer<float3> _Positions;
float _PositionsLength;
#endif


void PointDistanceToMultiline_float(in float2 Point, out float Distance, out float3 Min, out float3 Max) {
#ifdef SHADER_API_D3D11
	Distance = 100;
	Min = _Positions[0];
	Max = Min;

	for (float i = 0; i < _PositionsLength - 1; i += 2) {
		float3 pos0 = _Positions[i];
		float3 pos1 = _Positions[i + 1];

		float d;
		PointDistanceToLine_float(Point, pos0, pos1, d);
		Distance = min(Distance, d);
		Min = min(Min, pos1);
		Max = max(Max, pos1);
	}
#else
	Distance = 0;
#endif
}

void PointDistanceToPolyline_float(in float2 Point, out float Distance, out float3 Min, out float3 Max) {
#ifdef SHADER_API_D3D11
	Distance = 100;
	Min = _Positions[0];
	Max = Min;
	
	for (float i = 0; i < _PositionsLength - 1; ++i) {
		float3 pos0 = _Positions[i];
		float3 pos1 = _Positions[i + 1];

		float d;
		PointDistanceToLine_float(Point, pos0, pos1, d);
		Distance = min(Distance, d);
		Min = min(Min, pos1);
		Max = max(Max, pos1);
	}
#else
	Distance = 0;
#endif
}

void RayDistanceToPolyline_float(in float3 Origin, in float3 Direction, out float Distance, out float Depth, out float3 Min, out float3 Max) {
#ifdef SHADER_API_D3D11

	float3 nDir = normalize(Direction);

	Distance = 10000;
	Depth = 10000;
	Min = _Positions[0];
	Max = Min;

	for (float i = 0; i < _PositionsLength - 1; ++i) {
		float3 pos0 = _Positions[i];
		float3 pos1 = _Positions[i + 1];

		float d;
		float d2;
		RayDistanceToLine_float(Origin, nDir, pos0, pos1, d, d2);
		if (Distance > d) {
			Distance = d;
			Depth = d2;
		}
		Min = min(Min, pos1);
		Max = max(Max, pos1);
	}
#else
	Distance = 0;
#endif
}

void RayDistanceToPoints_float(in float3 Origin, in float3 Direction, out float Distance, out float Depth, out float3 Min, out float3 Max) {
#ifdef SHADER_API_D3D11
	Distance = 100;
	float3 nDir = normalize(Direction);
	Min = _Positions[0];
	Max = Min;

	for (float i = 0; i < _PositionsLength; i += 1) {
		float3 pos0 = _Positions[i];

		float d;
		float d2;
		RayDistanceToPoint_float(Origin, nDir, pos0, d, d2);
		if (Distance > d) {
			Distance = d;
			Depth = d2;
		}
		Min = min(Min, pos0);
		Max = max(Max, pos0);
	}
#else
	Distance = 0;
#endif
}