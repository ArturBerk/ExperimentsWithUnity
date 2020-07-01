#ifdef SHADER_API_D3D11
// DX11-specific code, e.g.
StructuredBuffer<float3> _Positions;
float _PositionsLength;
#endif

void GetPositionAt_float(in float T, out float3 Position, out float3 Direction) {
#ifdef SHADER_API_D3D11
	float t = T * (_PositionsLength - 1);
	float t1 = ceil(t);
	float t0 = floor(t);
	float3 pos1 = _Positions[t1];
	float3 pos0 = _Positions[t0];

	Position = lerp(pos0, pos1, (t - t0) / (t1 - t0));
	Direction = normalize(pos1 - pos0);
#endif
}

void Convert_float(in float3 Value, in float3 Direction, out float3 OutValue) {
	float3 forward = normalize(Direction);
	float3 up = normalize(cross(Direction, float3(1, 0, 0)));
	float3 right = normalize(cross(up, forward));

	OutValue = forward * Value.z + up * Value.y + right * Value.x;
}