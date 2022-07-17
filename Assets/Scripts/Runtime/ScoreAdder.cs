using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

namespace Dev.ComradeVanti.Wurfel
{

    public class ScoreAdder : MonoBehaviour
    {

        [SerializeField] private Transform cameraTransform;
        [SerializeField] private UnityEvent<int> onScoreAdded;
        [SerializeField] private TeamBaseKeeper redTeam;
        [SerializeField] private TeamBaseKeeper blueTeam;
        [SerializeField] private float riseHeight;
        [SerializeField] private float riseTime;
        
        private TeamBaseKeeper current;

        public void OnTurnTeamChanged(Team team) => 
            current = team == Team.Red ? redTeam : blueTeam;

        public Coroutine AddScoreToCurrentTeam(int score, Vector3 dicePosition)
        {
            IEnumerator Animation()
            {
                onScoreAdded.Invoke(score);

                var t = 0f;
                var transform = this.transform;
                
                while (t < 1)
                {
                    t = Mathf.MoveTowards(t, 1, Time.deltaTime / riseTime);
                    var dY = Mathf.Lerp(0, riseHeight, t);
                    
                    transform.position = dicePosition.MapY(y => y + dY);
                    transform.forward = (transform.position- cameraTransform.position).normalized;
                    
                    yield return null;
                }

                transform.position = new Vector3(0, -100, 0);
            }

            current.AddScore(score);
            return StartCoroutine(Animation());
        }

    }

}