float4x4 World;
float4x4 View;
float4x4 Projection;

float3 DiffuseColor = float3(1,1,1);
float3 AmbientColor = float3(.1, .1, .1);
float3 LightDirection = float3(1,1,1);
float3 LightColor = float3(0.9, 0.9, 0.9);

texture BasicTexture;

sampler BasicTextureSampler = sampler_state {
	texture = <BasicTexture>;
};

float SpecularPower = 32;
float3 SpecularColor = float3(1,1,1);
float3 CameraPosition;

bool TextureEnabled = false;

float ViewportWidth;
float ViewportHeight;

float4x4 ProjectorViewProjection;
texture2D ProjectedTexture;

sampler2D ProjectorSampler = sampler_state
{
	texture = <ProjectedTexture>;
};

float2 ProjectionToScreen(float4 position)
{
	float2 screenPos = position.xy / position.w;
	return 0.5f * (float2(screenPos.x, -screenPos.y) + 1);
}

float2 halfPixel()
{
	return 0.5f / float2(ViewportWidth, ViewportHeight);
}

float3 Project(float2 UV)
{
	if (UV.x < 0 || UV.x > 1 || UV.y < 0 || UV.y > 1)
		return float3(0,0,0);

	return tex2D(ProjectorSampler, UV);
}


//------------------------------------------------------------


bool ProjectorEnabled = false;

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
	float3 ViewDirection: TEXCOORD2;

	float4 ProjectorScreenPosition : TEXCOORD4;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	output.UV = input.UV;
	output.Normal = mul(input.Normal, World); 
	output.ViewDirection = worldPosition - CameraPosition;

	output.ProjectorScreenPosition = mul(mul(input.Position, World), 
		ProjectorViewProjection);

	return output;
}



float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float3 color = DiffuseColor;

	if (TextureEnabled)
		color *= tex2D(BasicTextureSampler, input.UV);

	float3 lighting = AmbientColor;
	float3 lightDir = normalize(LightDirection);
	float3 normal = normalize(input.Normal);

	lighting += saturate(dot(lightDir, normal)) * LightColor;

	float3 refl = reflect(lightDir, normal);
	float3 view = normalize(input.ViewDirection);
	lighting += pow(saturate(dot(refl, view)), SpecularPower) * SpecularColor;
	
	float3 output = saturate(lighting) * color;

	if (ProjectorEnabled)
		output += Project(ProjectionToScreen(input.ProjectorScreenPosition) + halfPixel());
	
	return float4(output, 1);
}

technique Technique1
{
	pass Pass1
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
