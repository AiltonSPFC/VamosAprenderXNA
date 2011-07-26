float4x4 World;
float4x4 View;
float4x4 Projection;

float3 AmbientLightColor = float3(.05, .05, .05);
float3 DiffuseColor = float3(.85, .85, .85);

float3 LightPosition = float3(0, 1500, 1500);
float3 LightColor = float3(1, 1, 1);
float LightAttenuation = 3000;

texture BasicTexture;

sampler BasicTextureSampler = sampler_state { 
	texture = <BasicTexture>; 
};

float SpecularPower = 32;
float3 SpecularColor = float3(1,1,1);
bool SpecularEnabled = true;
float3 CameraPosition;

bool TextureEnabled = true;

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
	float4 WorldPosition : TEXCOORD2;
	float3 ViewDirection : TEXCOORD3;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	output.WorldPosition = worldPosition;

	output.UV = input.UV;

	output.Normal = mul(input.Normal, World);
	output.ViewDirection = worldPosition - CameraPosition;
	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 color = DiffuseColor;

	if (TextureEnabled)
		color *= tex2D(BasicTextureSampler, input.UV).rgb;
	
	float3 lighting = float3(0, 0, 0);
	
	lighting += AmbientLightColor;
	
	float3 direction = normalize(LightPosition - input.WorldPosition);
	float diffuse = saturate(dot(normalize(input.Normal), direction));
	
	float d = distance(LightPosition, input.WorldPosition);
	float att = 1 - pow(clamp(d / LightAttenuation, 0, 1), 2); 

	lighting += diffuse * att * LightColor;

	if (SpecularEnabled)
	{
		float3 refl = reflect(direction, normalize(input.Normal));
		float3 view = normalize(input.ViewDirection);
		lighting += pow(saturate(dot(refl, view)), SpecularPower) * SpecularColor;
	}

	return float4(color * lighting, 1);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
