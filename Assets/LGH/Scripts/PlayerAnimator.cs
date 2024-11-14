using GH;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


[System.Serializable]
public struct AvatarSprite
{
    public List<Sprite> frontAnimations;
    public List<Sprite> backAnimations;
    public List<Sprite> sideAnimations;
    public List<Sprite> idleAnimations;
}


public class PlayerAnimator : MonoBehaviourPun, IPunObservable
{
    //[����][����][��,��,���̵�]
    public List<AvatarSprite> skin;
    public List<AvatarSprite> clothes;
    public List<AvatarSprite> face;
    public List<AvatarSprite> hair;

    public SpriteRenderer skinSpriteRenderer;
    public SpriteRenderer clothesSpriteRenderer;
    public SpriteRenderer faceSpriteRenderer;
    public SpriteRenderer hairSpriteRenderer;

    public int skinid;
    public int clothesid;
    public int faceid;
    public int hairid;

    public PlayerMove playerMove;

    //�ִϸ��̼� ����
    float frameRate = 0.1f;
    int currentFrame = 0;
    float timer = 0f;
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();

    }

    void Update()
    {
        UdateTime();
        if (playerMove.onIdle)
        {
            frameRate = 0.2f;
            SpriteAnimation(clothes[clothesid].idleAnimations, clothesSpriteRenderer);
            SpriteAnimation(skin[skinid].idleAnimations, skinSpriteRenderer);
            SpriteAnimation(face[faceid].idleAnimations, faceSpriteRenderer);
            SpriteAnimation(hair[hairid].idleAnimations, hairSpriteRenderer);

           // Debug.Log("clothes: " + clothesid + "skin: " + skinid + "face: " + faceid + "hair:" + hairid);
        }
        else
        {
            frameRate = 0.1f;

            if (playerMove.stingDir == -transform.up)
            {
                SpriteAnimation(clothes[clothesid].frontAnimations, clothesSpriteRenderer);
                SpriteAnimation(skin[skinid].frontAnimations, skinSpriteRenderer);
                SpriteAnimation(face[faceid].frontAnimations, faceSpriteRenderer);
                SpriteAnimation(hair[hairid].frontAnimations, hairSpriteRenderer);
            }
            else if (playerMove.stingDir == transform.up)
            {
                SpriteAnimation(clothes[clothesid].backAnimations, clothesSpriteRenderer);
                SpriteAnimation(skin[skinid].backAnimations, skinSpriteRenderer);
                SpriteAnimation(face[faceid].backAnimations, faceSpriteRenderer);
                SpriteAnimation(hair[hairid].backAnimations, hairSpriteRenderer);
            }
            else if (playerMove.stingDir == transform.right)
            {
                SpriteAnimation(clothes[clothesid].sideAnimations, clothesSpriteRenderer);
                SpriteAnimation(skin[skinid].sideAnimations, skinSpriteRenderer);
                SpriteAnimation(face[faceid].sideAnimations, faceSpriteRenderer);
                SpriteAnimation(hair[hairid].sideAnimations, hairSpriteRenderer);

                clothesSpriteRenderer.flipX = false;
                skinSpriteRenderer.flipX = false;
                faceSpriteRenderer.flipX = false;
                hairSpriteRenderer.flipX = false;
            }
            else if (playerMove.stingDir == -transform.right)
            {
                SpriteAnimation(clothes[clothesid].sideAnimations, clothesSpriteRenderer);
                SpriteAnimation(skin[skinid].sideAnimations, skinSpriteRenderer);
                SpriteAnimation(face[faceid].sideAnimations, faceSpriteRenderer);
                SpriteAnimation(hair[hairid].sideAnimations, hairSpriteRenderer);
                clothesSpriteRenderer.flipX = true;
                skinSpriteRenderer.flipX = true;
                faceSpriteRenderer.flipX = true;
                hairSpriteRenderer.flipX = true;

            }

        }

    }
    [PunRPC]
    public void SetAvatarPart(int skinId, int clothesId, int faceId, int hairId)
    {
        skinid = skinId;
        clothesid = clothesId;
        faceid = faceId;
        hairid = hairId;
    }

    private void SpriteAnimation(List<Sprite> sprites, SpriteRenderer spriteRenderer)
    {
        spriteRenderer.sprite = sprites[currentFrame];
    }

    private void UdateTime()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            timer = 0;
            currentFrame++;

            if (currentFrame >= 4)
            {
                currentFrame = 0; // �ٽ� ó�� ����������
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ���� �����͸� ������ ����(PhotonView.IsMine == true)�ϴ� ���¶��
        if (stream.IsWriting)
        {

        }
        //�׷��� �ʰ� ���� �����͸� �����κ��� �о���� ���¶��
        else if (stream.IsReading)
        {

        }
    }
}
