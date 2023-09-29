using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    enum CurrentPlayer
    {
        Player1,
        Player2
    }
    CurrentPlayer currentPlayer;
    bool isWinningShotforPlayer1 = false;
    bool isWinningShotforPlayer2 = false;
    int player1BallsRemaining = 7;
    int player2BallsRemaining = 7;

    [SerializeField] TextMeshProUGUI player1BallsText;
    [SerializeField] TextMeshProUGUI player2BallsText;
    [SerializeField] TextMeshProUGUI currentTurnText;
    [SerializeField] TextMeshProUGUI messageText;

    [SerializeField] GameObject restartButton;

    [SerializeField] Transform headPosition;
    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = CurrentPlayer.Player1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void RestartTheGame()
    {
        SceneManager.LoadScene(0);
    }
    bool Scratch()
    {
        if(currentPlayer == CurrentPlayer.Player1)
        {
            if (isWinningShotforPlayer1)
            {
                ScratchOnWinningShot("Player 1");
                return true;
            }
        }
        else
        {
            if (isWinningShotforPlayer2)
            {
                ScratchOnWinningShot("Player 2");
                return true;
            }
        }
        nextPlayerturn();
        return false;
        

    }
    void EarlyEightBall()
    {
        if(currentPlayer == CurrentPlayer.Player1)
        {
            Lose("Player 1 hit the 8 ball too early and lost!");
        }
        else
        {
            Lose("Player 2 hit the 8 ball too early and lost!");
        }

    }
    void ScratchOnWinningShot(string player)
    {
        Lose(player + "Scratched on winning shot and lost");

    }
    void NomoreBalls(CurrentPlayer player)
    {
        if(player == CurrentPlayer.Player1)
        {
            isWinningShotforPlayer1 = true;
        }
        else
        {
            isWinningShotforPlayer2 = true;
        }

    }
    bool checkBall(Ball ball)
    {
        if(ball.IsCueBall())
        {
            if(Scratch())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (ball.Is8Ball())
        {
            if(currentPlayer == CurrentPlayer.Player1)
            {
                if (isWinningShotforPlayer1)
                {
                    Win("Player 1");
                    return true;
                }
            }
            else
            {
                if (isWinningShotforPlayer2)
                {
                    Win("Player 2");
                    return true;
                }
            }
            EarlyEightBall();
          

        }
        else
        {
            if(ball.isBallRed())
            {
                player1BallsRemaining--;
                player1BallsText.text = "Player 1 Balls Remaining: " + player1BallsRemaining;
                if(player1BallsRemaining <= 0)
                {
                    isWinningShotforPlayer1 = true;
                }
                if(currentPlayer != CurrentPlayer.Player1)
                {
                    nextPlayerturn();
                }
            }
            else
            {
                player2BallsRemaining--;
                player2BallsText.text = "Player 2 Balls Remaining: " + player2BallsRemaining;
                if (player2BallsRemaining <= 0)
                {
                    isWinningShotforPlayer2 = true;
                }
                if(currentPlayer != CurrentPlayer.Player2)
                {
                    nextPlayerturn();
                }
            }
        }
        return true;

    }
    void Lose(string message)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = message;
        restartButton.SetActive(true);

    }
    void Win(string player)
    {
        messageText.gameObject.SetActive(true);
        messageText.text = player + "Has Won";
        restartButton.SetActive(true);

    }
    void nextPlayerturn()
    {
        if(currentPlayer == CurrentPlayer.Player1)
        {
            currentPlayer = CurrentPlayer.Player2;
            currentTurnText.text = "Current Turn: Player 2";
        }
        else
        {
            currentPlayer = CurrentPlayer.Player1;
            currentTurnText.text = "Current Turn: Player 1";
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            if(checkBall(other.gameObject.GetComponent<Ball>()))
            {
                Destroy(other.gameObject);
            }
            else
            {
                other.gameObject.transform.position = headPosition.position;
                other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                
            }
        }
    }
}
