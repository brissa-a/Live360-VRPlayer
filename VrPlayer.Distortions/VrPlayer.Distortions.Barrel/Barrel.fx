// Based on Doom 3 BFG Edition GPL Source Code: Copyright (C) 1993-2012 id Software LLC, a ZeniMax Media company. 

sampler2D input : register(s0);

//TODO: Use param to set screenWarp_range
float factor: register(C0); 

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	float screenWarp_range = 1.45;
	const float2 warpCenter = float2( 0.5, 0.5 );
  
  float2 centeredTexcoord = float2(uv.x - warpCenter.x, uv.y - warpCenter.y);

	float	radialLength = length( centeredTexcoord );
	float2	radialDir = normalize( centeredTexcoord );

	// get it down into the 0 - PI/2 range
	float	range = screenWarp_range;
	float	scaledRadialLength = radialLength * range;
	float	tanScaled = tan( scaledRadialLength );

	float rescaleValue = tan( 0.5 * range );

  // If radialLength was 0.5, we want rescaled to also come out
  // as 0.5, so the edges of the rendered image are at the edges
  // of the warped image.
	float	rescaled = tanScaled / rescaleValue;

	float warpedX = warpCenter.x + (0.5 * radialDir.x * rescaled);
	float warpedY = warpCenter.y + (0.5 * radialDir.y * rescaled);
  float2 warped = float2(warpedX, warpedY);

	return tex2D( input, warped );
}