using System;
using System.Collections.Generic;

public class ViSpeakManager
{
	public static ViSpeakManager Instance;

	Dictionary<Int32, ViSpeak> _speaks = new Dictionary<Int32, ViSpeak>();
}
