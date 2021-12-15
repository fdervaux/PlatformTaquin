using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Taquin : MonoBehaviour
{

    public List<GameObject> _gameobjects;

    public GameObject[,] _grid = new GameObject[4, 4];

    public float ElementSize = 20;

    private MoveToTarget currentMoveToTarget = null;





    //public PlayerInput _playerInput;

    public void OnClick(InputValue value)
    {
        if (currentMoveToTarget == null || currentMoveToTarget.isReachedTarget())
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                GameObject objectToMove = hit.transform.gameObject;


                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (objectToMove == _grid[i, j])
                        {
                            tryMove(i, j);
                            Debug.Log(i + " , " + j);
                            return;
                        }
                    }
                }
            }
        }

    }

    public void tryMove(int i, int j)
    {
        if (i > 0 && _grid[i - 1, j] == null)
        {
            move(i, j, i - 1, j);
        }
        if (i < 2 && _grid[i + 1, j] == null)
        {
            move(i, j, i + 1, j);
        }
        if (j > 0 && _grid[i, j - 1] == null)
        {
            move(i, j, i, j - 1);
        }
        if (j < 2 && _grid[i, j + 1] == null)
        {
            move(i, j, i, j + 1);
        }

    }

    public void move(int i, int j, int newI, int newJ)
    {
        _grid[newI, newJ] = _grid[i, j];
        _grid[i, j] = null;

        currentMoveToTarget = _grid[newI, newJ].GetComponent<MoveToTarget>();

        if (currentMoveToTarget != null)
        {
            currentMoveToTarget.setTarget(getPosition(newI, newJ));
        }
    }


    Vector3 getPosition(int i, int j)
    {
        return transform.position + new Vector3((j - 1) * ElementSize, (1 - i) * ElementSize, 0);
    }

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

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 2 && j == 2)
                {
                    break;
                }
                Vector3 position = getPosition(i, j);
                _grid[i, j] = GameObject.Instantiate(_gameobjects[i * 3 + j], position, Quaternion.identity, transform);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {

    }
}
