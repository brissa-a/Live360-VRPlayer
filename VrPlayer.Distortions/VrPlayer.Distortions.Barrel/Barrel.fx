// Based on Doom 3 BFG Edition GPL Source Code: Copyright (C) 1993-2012 id Software LLC, a ZeniMax Media company. 

sampler2D input : register(s0);

float factor: register(C0); 

float4 main(float2 uv : TEXCOORD) : COLOR 
{ 
	const float2 warpCenter = float2( 0.5, 0.5 );
  
    float2 centeredTexcoord = float2(uv.x - warpCenter.x, uv.y - warpCenter.y);

	float2	warped = normalize( centeredTexcoord );

	// get it down into the 0 - PI/2 range

  // If radial length was 0.5, we want rescaled to also come out
  // as 0.5, so the edges of the rendered image are at the edges
  // of the warped image.
	float	rescaled = tan( length( centeredTexcoord ) * factor ) / tan( 0.5 * factor );

	warped *= 0.5 * rescaled;
	warped += warpCenter;

	return tex2D( input, warped );
}