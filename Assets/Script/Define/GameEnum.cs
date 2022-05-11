using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnum
{
    // Fire > Wind > Water > Fire
    public enum eRoleAttribute
    {
        E_ROLE_ATTRIBUTE_NA = 0,
        E_ROLE_ATTRIBUTE_POWER,      // Fire(Red)
        E_ROLE_ATTRIBUTE_SPEED,      // Wind(Green)
        E_ROLE_ATTRIBUTE_TECHNIQUE,  // Water(Blue)
        E_ROLE_ATTRIBUTE_LIMIT,
    }
}
