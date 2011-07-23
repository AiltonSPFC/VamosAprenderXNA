float4x4 World;
float4x4 View;
float4x4 Projection;

float3 DiffuseColor = float3(1,1,1);

texture BasicTexture;

sampler BasicTextureSampler = sampler_state {
	texture = <BasicTexture>;
};

bool TextureEnabled = false;


struct VertexShaderInput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};

struct VertexShaderOutput
{
	float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	output.UV = input.UV;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	
	if (TextureEnabled)
	{
		float3 output = float3(1,1,1);
		output *= tex2D(BasicTextureSampler, input.UV);
		return float4(output, 1);
	}
	else
	{
		return float4(DiffuseColor, 1);
	}
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
