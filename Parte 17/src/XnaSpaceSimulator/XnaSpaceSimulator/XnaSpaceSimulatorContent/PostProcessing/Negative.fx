sampler TextureSampler;

struct PixelInput
{
    float2 TexCoord : TEXCOORD0;
};

float4 PixelShaderFunction(PixelInput input) : COLOR
{
	return 1 - tex2D(TextureSampler, input.TexCoord);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
