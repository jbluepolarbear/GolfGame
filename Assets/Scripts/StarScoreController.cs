using System.Collections;
using System.Collections.Generic;
using Contexts;
using UnityEngine;
using UnityEngine.UI;

public class StarScoreController : MonoBehaviour
{
    [SerializeField]
    private StarScore[] _stars;
    
    public void SetStarScore(int starScore, bool halfHide = false)
    {
        for (int i = 0; i < _stars.Length; ++i)
        {
            if (i < starScore)
            {
                _stars[i].Show();
            }
            else
            {
                if (halfHide)
                {
                    _stars[i].HalfHide();
                }
                else
                {
                    _stars[i].Hide();
                }
            }
        }
    }
    
    
}
