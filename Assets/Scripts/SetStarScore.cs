using System.Collections;
using System.Collections.Generic;
using Contexts;
using UnityEngine;

public class SetStarScore : MonoBehaviour
{
    [SerializeField]
    private StarScoreController _starScoreController;
    
    // Start is called before the first frame update
    void Update()
    {
        _starScoreController.SetStarScore(Context.Get<GameController>().StarScore, true);
    }
}
