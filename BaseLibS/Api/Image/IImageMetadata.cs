using System;
using System.Collections.Generic;
using System.Text;

namespace BaseLibS.Api.Image {
    public interface IImageMetadata : ICloneable {
        double[,,] RigidBodyTransformation { get; set; }
        string[,] Events { get; set; }
        string GetBIDSEntity(string entityName);
    }
}
