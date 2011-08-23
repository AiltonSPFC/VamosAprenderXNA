sampler TextureSampler;

struct PixelInput
{
    float2 TexCoord : TEXCOORD0;
};

float4 PixelShaderFunction(PixelInput input) : COLOR
{
	float4 color = tex2D(TextureSampler, input.TexCoord);
	color.rgb = 0.5f;
	color.a = 1;
	color += tex2D(TextureSampler, input.TexCoord - 0.0001) * 15.0f;
	color -= tex2D(TextureSampler, input.TexCoord + 0.0001) * 15.0f;
	color = (color.r + color.g + color.b) / 3.0f;
	return color; 
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
