sampler TextureSampler;

struct PixelInput
{
    float2 TexCoord : TEXCOORD0;
};

float4 PixelShaderFunction(PixelInput input) : COLOR
{
	float4 color = tex2D(TextureSampler, input.TexCoord);
	color = (color.r + color.g + color.b) / 3.0f;

	if (color.r < 0.2 || color.r > 0.8) 
		color.rgb = 0.0f; 
	else 
		color.rgb = 1.0f; 
	return color; 
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
