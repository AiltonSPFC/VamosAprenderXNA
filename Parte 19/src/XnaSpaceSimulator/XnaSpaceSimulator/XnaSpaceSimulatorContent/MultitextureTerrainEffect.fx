
texture RTexture;
sampler RTextureSampler = sampler_state
{
	texture = <RTexture>;
	AddressU = Wrap;
	AddressV = Wrap;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
};

texture GTexture;
sampler GTextureSampler = sampler_state
{
	texture = <GTexture>;
	AddressU = Wrap;
	AddressV = Wrap;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
};

texture BTexture;
sampler BTextureSampler = sampler_state
{
	texture = <BTexture>;
	AddressU = Wrap;
	AddressV = Wrap;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
};

texture BaseTexture;
sampler BaseTextureSampler = sampler_state
{
	texture = <BaseTexture>;
	AddressU = Wrap;
	AddressV = Wrap;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
};

texture WeightMap;
sampler WeightMapSampler = sampler_state
{
	texture = <WeightMap>;
	AddressU = Wrap;
	AddressV = Wrap;
	MinFilter = Anisotropic;
	MagFilter = Anisotropic;
};


// ---------------------------------------------------------

float4x4 World;
float4x4 View;
float4x4 Projection;

float3 LightDirection = float3(1, -1, 0);
float TextureTiling = 6;

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

// ---------------------------------------------------------

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

	float3 rtex = tex2D(RTextureSampler, input.UV * TextureTiling);
	float3 gtex = tex2D(GTextureSampler, input.UV * TextureTiling);
	float3 btex = tex2D(BTextureSampler, input.UV * TextureTiling);
	float3 base = tex2D(BaseTextureSampler, input.UV * TextureTiling);

	float3 weightMap = tex2D(WeightMapSampler, input.UV);

	float3 output = clamp(1.0f - weightMap.r - weightMap.g - weightMap.b, 0, 1);
	output *= base;

	output += weightMap.r * rtex + weightMap.g * gtex + weightMap.b * btex;

	return float4(output * light, 1);
}

technique Technique1
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
