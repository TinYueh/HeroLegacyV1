using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Role
{
    public int Id { get; protected set; } = 0;
    public int Portrait { get; protected set; } = 0;
    public int Emblem { get; protected set; } = 0;
    public int Name { get; protected set; } = 0;

    public abstract bool Init(int id);
}
