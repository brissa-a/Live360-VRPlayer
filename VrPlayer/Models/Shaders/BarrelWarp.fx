texture input;

sampler2D inputSampler : register(S0);

float barrelFactor: register(C0); 

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float2 v = float2(uv);
    v *= 2.0; //[0,1]-based -> [-1,1]
    v -= 1.0;
    float2 warped = float2(
        barrelFactor*v.x/(v.y*v.y + barrelFactor), 
        barrelFactor*v.y/(v.x*v.x + barrelFactor));
    warped += 1.0; //[-1,1] back into [0,1]
    warped *= 0.5;

    return tex2D(inputSampler, warped);
}

technique RenderBarrelWarp
{
    pass p0
    {
        VertexShader = null;
        PixelShader = compile ps_2_0 main();
        ZEnable = false;
    }
}