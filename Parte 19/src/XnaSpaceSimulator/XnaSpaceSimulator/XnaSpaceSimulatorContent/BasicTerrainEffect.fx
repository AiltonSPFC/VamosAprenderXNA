float4x4 World;
float4x4 View;
float4x4 Projection;

float3 LightDirection = float3(1, -1, 0);

float TextureTiling = 6;
texture2D Texture;
sampler2D TextureSampler = sampler_state {
	Texture = <Texture>;
	AddressU = Wrap;
	AddressV = Wrap;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 UV : TEXCOORD0;
	float3 Normal : TEXCOORD1;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	output.Normal = input.Normal;
	output.UV = input.UV;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float light = dot(
		normalize(input.Normal),
		normalize(LightDirection)
		);
	
	light = clamp(light + 0.4f, 0, 1);

	float3 tex = tex2D(TextureSampler, input.UV * TextureTiling);

	return float4(tex * light, 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
