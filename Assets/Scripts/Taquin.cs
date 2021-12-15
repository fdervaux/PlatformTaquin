using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taquin : MonoBehaviour
{

    public List<GameObject> _gameobjects;

    public GameObject[,] _grid = new GameObject[4,4]; 

    public float ElementSize = 20;

    // Start is called before the first frame update
    void Start()
    {
        /*_grid[0,0] = _gameobjects[0];
        
        _grid[0,1] = _gameobjects[1];
        
        _grid[0,2] = _gameobjects[2];

        _grid[1,0] = _gameobjects[3+0];
        
        _grid[1,1] = _gameobjects[3+1];
        
        _grid[1,2] = _gameobjects[3+2];

        _grid[2,0] = _gameobjects[2*3+0];
    
        _grid[2,1] = _gameobjects[2*3+1];
        
        _grid[2,2] = _gameobjects[2*3+2];*/

        for(int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3 position = transform.position + new Vector3((j-1)* ElementSize, (i-1) * ElementSize, 0); 
                _grid[i,j] = GameObject.Instantiate(_gameobjects[i*3+j],position,Quaternion.identity,transform);       
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
