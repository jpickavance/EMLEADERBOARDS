using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class Metrics : MonoBehaviour

{
  public GammaPercentiles gammaPercentiles;
  public double tranSec;
  public double aveWPM;
  public double CalcWPM(DateTime startTime, DateTime endTime, double numChars)
  {
      // transcription time in seconds = tranSec
      tranSec = Math.Round((endTime - startTime).TotalSeconds, 0);

      double wpm = Math.Round(((numChars - 1) / tranSec) * 60 * 1/5, 0);

      tranSec = 0;
      return wpm;
  }

public double CalcAveWPM(double[] wpmArray)
{
  aveWPM = Math.Round(wpmArray.Average());
  return aveWPM;

  // decided to create aveWPM and make it public so it's visible in the inspector

}

public double CalcArrowXPos(double aveWPM)
{
  // 0-150 WPM spans 711 pixels, or 6 unity units (118.5ppu)
  return (aveWPM * 0.04) - 3;
}

public double CalcAveWPMPercentile(double aveWPM)
{
  return gammaPercentiles.gammaPercs[Convert.ToInt32(aveWPM)-1];
}

}