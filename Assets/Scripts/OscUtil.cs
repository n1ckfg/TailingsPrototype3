using UnityEngine;
using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

public static class OscUtil {

	public static Color asColor(byte[] bytes) {
		byte[] rBytes = { bytes[0], bytes[1], bytes[2], bytes[3] };
		byte[] gBytes = { bytes[4], bytes[5], bytes[6], bytes[7] };
		byte[] bBytes = { bytes[8], bytes[9], bytes[10], bytes[11] };
		float r = BitConverter.ToSingle(rBytes, 0);
		float g = BitConverter.ToSingle(gBytes, 0);
		float b = BitConverter.ToSingle(bBytes, 0);
		return new Color(r, g, b);
	}

	public static List<Vector3> asPoints(byte[] bytes) {
		List<Vector3> returns = new List<Vector3>();

		for (int i = 0; i < bytes.Length; i += 12) {
			byte[] xBytes = { bytes[i], bytes[i + 1], bytes[i + 2], bytes[i + 3] };
			byte[] yBytes = { bytes[i + 4], bytes[i + 5], bytes[i + 6], bytes[i + 7] };
			byte[] zBytes = { bytes[i + 8], bytes[i + 9], bytes[i + 10], bytes[i + 11] };
			float x = BitConverter.ToSingle(xBytes, 0);
			float y = BitConverter.ToSingle(yBytes, 0);
			float z = BitConverter.ToSingle(zBytes, 0);
			returns.Add(new Vector3(x, y, z));
		}

		return returns;
	}

	public static byte[] floatsToBytes(float[] floats) {
		MemoryStream stream = new MemoryStream();
		BinaryWriter bw = new BinaryWriter(stream);
		for (int i = 0; i < floats.Length; i++) {
			bw.Write(floats[i]);
		}
		bw.Flush();
		byte[] returns = stream.ToArray();
		return returns;
	}

	public static float[] bytesToFloats(byte[] bytes) {
		MemoryStream stream = new MemoryStream(bytes);
		BinaryReader br = new BinaryReader(stream);
		int len = (int)(bytes.Length / 4);
		float[] returns = new float[len];
		for (int i = 0; i < len; i++) {
			returns[i] = br.ReadSingle();
		}
		return returns;
	}

	public static Color bytesToColor(byte[] bytes) {
		MemoryStream stream = new MemoryStream(bytes);
		BinaryReader br = new BinaryReader(stream);
		Vector3 v = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()) / 255f;

		return new Color(v.x, v.y, v.z);
	}

	public static List<Color> bytesToColors(byte[] bytes) {
		List<Color> returns = new List<Color>();

		MemoryStream stream = new MemoryStream(bytes);
		BinaryReader br = new BinaryReader(stream);
		int len = (int)(bytes.Length / 12);
		for (int i = 0; i < len; i++) {
			Vector3 v = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()) / 255f;
			returns.Add(new Color(v.x, v.y, v.z));
		}
		return returns;
	}

	public static Vector3 bytesToVec3(byte[] bytes) {
		MemoryStream stream = new MemoryStream(bytes);
		BinaryReader br = new BinaryReader(stream);
		return new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()) / 500f;
	}

	public static List<Vector3> bytesToVec3s(byte[] bytes) {
		List<Vector3> returns = new List<Vector3>();

		MemoryStream stream = new MemoryStream(bytes);
		BinaryReader br = new BinaryReader(stream);
		int len = (int)(bytes.Length / 12);
		for (int i = 0; i < len; i++) {
			Vector3 v = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()) / 500f;
			returns.Add(new Vector3(v.x, v.y, v.z));
		}
		return returns;
	}

}
