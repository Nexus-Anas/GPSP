using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPSP;

public class GeocodingResponse
{
    public string status { get; set; }
    public GeocodingResult[] results { get; set; }
}

public class GeocodingResult
{
    public Geometry geometry { get; set; }
}

public class Geometry
{
    public Loc location { get; set; }
}

public class Loc
{
    public double lat { get; set; }
    public double lng { get; set; }
}
