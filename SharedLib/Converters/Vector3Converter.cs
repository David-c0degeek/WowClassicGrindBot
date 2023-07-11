﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Numerics;

namespace SharedLib.Converters;

public sealed class Vector3Converter : JsonConverter<Vector3>
{
    private const string X = "x";
    private const string Y = "y";
    private const string Z = "z";

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(Vector3);
    }

    public override Vector3 Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        JsonDocument doc = JsonDocument.ParseValue(ref reader);
        JsonElement root = doc.RootElement;

        float x = root.GetProperty(X).GetSingle();
        float y = root.GetProperty(Y).GetSingle();
        float z = root.GetProperty(Z).GetSingle();

        return new Vector3(x, y, z);
    }

    public override void Write(Utf8JsonWriter writer,
        Vector3 value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber(X, value.X);
        writer.WriteNumber(Y, value.Y);
        writer.WriteNumber(Z, value.Z);
        writer.WriteEndObject();
    }
}
