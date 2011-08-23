sampler TextureSampler;

struct PixelInput
{
    float2 TexCoord : TEXCOORD0;
};

float4 PixelShaderFunction(PixelInput input) : COLOR
{
	float y = input.TexCoord.y;
	float x = input.TexCoord.x;
	y = y + (sin(x * 200) * 0.01);
	float4 color = tex2D(TextureSampler, float2(x,y));
	return color; 
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
