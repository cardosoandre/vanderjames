using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DialogSystem : MonoBehaviour {
    public static DialogSystem Instance { get; private set; }


    public Image charImage;
    public SuperTextMesh charNameText;
    public SuperTextMesh charLineText;

    private int count = 0;
    public string[] lines;

    public List<Character> characters;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Read("@jorge",lines[0]);
    }

    private void Update()
    {

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    count++;
        //    Read("@post",lines[count]);
        //}

        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        //}
    }

    public void Read(string reader, string line){

        CharLookup(reader, line);
    }

    private void PlaceLookup (){
        
    }

    private void CharLookup (string reader, string line){

        for (int i = 0; i < characters.Count; i++){
            if(reader.Contains(characters[i].charRef)){
                reader = reader.Replace(characters[i].charRef, characters[i].charName);
                if (charImage.sprite != characters[i].charSprite)
                {
                    charImage.sprite = characters[i].charSprite;
                    charImage.transform.DOScale(Vector3.right, 0.3f).From().SetEase(Ease.OutBack);
                }
            }
            if(line.Contains(characters[i].charRef)){
                line =  line.Replace(characters[i].charRef, "<b>" + characters[i].charName + "</b>");
            }
        }

        ReadLine(reader,line);
        
    }

    private void ReadLine(string reader, string line){
        charLineText.text = line;
        charLineText.RegularRead();
        charNameText.text = reader;
        print("Reader: " + reader);
    }

}

[System.Serializable]
public class Character {
    public string charName;
    public string charRef;
    public Sprite charSprite;
}

[System.Serializable]
public class Place
{
    public string placeName;
    public string placeRef;
}
