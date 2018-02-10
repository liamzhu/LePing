using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region 用户行为检测包 OperationBehaviorResp

[System.Serializable]
public class OperationBehaviorResp
{
    public List<OperationInfo> OperationInfos;
    public List<Card> ListenCards;

    public OperationBehaviorResp getCopy()
    {
        return (OperationBehaviorResp)MemberwiseClone();
    }
}

public enum OperationType
{
    Chi = 1,
    Peng = 2,
    Gang = 3,
    Hu = 5,
    Guo = 6
}

[System.Serializable]
public class OperationInfo
{
    public OperationType OperateType;
    public List<CardGroup> CardGroups;

    public OperationInfo()
    {

    }

    public OperationInfo(int type, List<CardGroup> cards)
    {
        this.OperateType = (OperationType)type;
        this.CardGroups = cards;
    }
}

#endregion 用户行为检测包 OperationBehaviorResp

#region 麻将牌 Card

[Serializable, ProtoContract]
public class Card
{
    /// <summary>
    /// 牌面类型
    /// </summary>
    [ProtoMember(1)]
    public int CardType { set; get; }

    /// <summary>
    /// 牌面大小
    /// </summary>
    [ProtoMember(2)]
    public int CardValue { set; get; }

    /// <summary>
    /// 标记
    /// </summary>
    [ProtoMember(3)]
    public int Flag { set; get; }

    /// <summary>
    /// 是否是万能牌
    /// </summary>
    [ProtoMember(4)]
    public bool IsUniversal { set; get; }

    public Card()
    {

    }

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    public Card(int type, int value)
    {
        CardType = type;
        CardValue = value;
    }

    public Card getCopy()
    {
        return (Card)MemberwiseClone();
    }

    /// <summary>
    /// 比较是否一样
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public bool Equals(Card card)
    {
        if (card == null) return false;
        if (card.CardType == CardType && card.CardValue == CardValue) return true;
        return false;
    }
}

public enum MahjongCardType
{
    Wan,
    Tong,
    Tiao,
    Feng,
    Jian,
    Hua,
}

public enum CardGroupType
{
    ABC,
    AAA,
    AAAA,
    Abc,
}

[Serializable, ProtoContract]
public class CardGroup
{
    /// <summary>
    /// 牌组类型
    /// </summary>
    [ProtoMember(1)]
    public int CardGroupType { get; set; }

    /// <summary>
    /// 牌组牌面
    /// </summary>
    [ProtoMember(2)]
    public List<Card> Cards { get; set; }

    /// <summary>
    /// 玩家ID
    /// </summary>
    [ProtoMember(3)]
    public int PlayerOrder { get; set; }

    /// <summary>
    /// 是否明牌
    /// </summary>
    [ProtoMember(4)]
    public bool IsClear { get; set; }

    public CardGroup()
    {

    }

    /// <summary>
    /// 构造
    /// </summary>
    /// <param name="type"></param>
    /// <param name="isClear">是否明牌</param>
    /// <param name="cards">牌组牌面</param>
    /// <param name="playerOrder">玩家序号</param>
    public CardGroup(int type, bool isClear, List<Card> cards, int playerOrder)
    {
        this.Cards = cards;
        CardGroupType = type;
        PlayerOrder = playerOrder;
        IsClear = isClear;
    }
}

#endregion 麻将牌 Card

public class DissolveRoomResp
{
    /// <summary> 是否投票结束 </summary>
    public bool IsEnd;

    /// <summary> 是否离开房间 </summary>
    public bool IsLeave;

    /// <summary> 是否操作过 </summary>
    public bool IsOpe;

    /// <summary> 申请人 </summary>
    public string ReqName;

    /// <summary> 同意申请的人 </summary>
    public string AgreeUser;

    /// <summary> 不同意申请的人 </summary>
    public string DisagreeUser;
}

public struct OperationAction
{
    public int RoomOrder;
    public MahJongGameAction Action;

    public OperationAction(int order, MahJongGameAction action)
    {
        this.RoomOrder = order;
        this.Action = action;
    }
}

public struct PlayerAction
{
    public int RoomOrder;
    public UserInfo User;

    public PlayerAction(int id, UserInfo user)
    {
        this.RoomOrder = id;
        this.User = user;
    }
}

[Serializable, ProtoContract]
public class MahJongGameAction
{
    /// <summary> 行为 </summary>
    [ProtoMember(1)]
    public MahJongActionType Action { get; set; }

    /// <summary> 辅助参数 </summary>
    [ProtoMember(2)]
    public int Parame { get; set; }

    /// <summary> 操作牌 </summary>
    [ProtoMember(3)]
    public List<Card> Cards { get; set; }

    [ProtoMember(4)]
    public int UserID { get; set; }

    /// <summary> 等待时间 </summary>
    [ProtoMember(5)]
    public int Timer { get; set; }

    /// <summary> 剩余牌张数 </summary>
    [ProtoMember(6)]
    public int CardsNo { get; set; }

    /// <summary> 起手翻牌或结束时翻牌判断的特殊用的牌 </summary>
    [ProtoMember(7)]
    public List<Card> OtherCards { get; set; }

    [ProtoMember(8)]
    public int RoundID { get; set; }

    /// <summary>
    /// 座位号
    /// </summary>
    /// <value>The Room Order.<alue>
    [ProtoMember(9)]
    public int Order { get; set; }

    public MahJongGameAction()
    {

    }

    public MahJongGameAction getCopy()
    {
        return (MahJongGameAction)MemberwiseClone();
    }
}

public enum MahJongActionType
{

    /// <summary> 吃牌 </summary>
    ChiCard = 1,

    /// <summary> 碰牌 </summary>
    PengCard = 2,

    /// <summary> 杠牌 </summary>
    GangCard = 3,

    /// <summary> 胡牌 </summary>
    HuCard = 5,

    /// <summary> 过 </summary>
    Guo = 6,

    /// <summary> 开局摸牌 </summary>
    StartTouchCard,

    /// <summary> 摸牌 </summary>
    TouchCard,

    /// <summary> 出牌 </summary>
    PlayCard,

    /// <summary> 起手翻牌 </summary>
    StartOpen,

    /// <summary> 结束翻牌 </summary>
    EndOpen,
}

public class ReConnectDataResp
{
    /// <summary> 当前该操作玩家 </summary>
    public MahJongGameAction LastAction;

    /// <summary> 特殊牌 </summary>
    public Dictionary<string, Card> SpCards;

    /// <summary> 玩家数据 </summary>
    public List<PlayerData> Players;

    /// <summary> 游戏行为 </summary>
    public List<MahJongGameAction> Actions;

    /// <summary> 该玩家检测结果 </summary>
    //public MahjongCheckResult CheckResult;

    public ReConnectDataResp()
    {

    }
}

public class PlayerData
{
    /// <summary> 玩家ID </summary>
    public int UID;

    /// <summary>
    /// 玩家序号
    /// </summary>
    public int Order;

    /// <summary> 手牌 </summary>
    public List<Card> HandCards;

    /// <summary> 手牌牌组 </summary>
    public List<CardGroup> HandCardGroups;

    /// <summary> 当前摸牌 </summary>
    public Card CurCard;

    /// <summary> 桌面牌 </summary>
    public List<Card> DeskCards;
}

#region 结算 GameEndActionResp

public class GameEndActionResp
{
    public RoundEndType Type;
    public SingleSettlementResp SingleSettlement;
    public GameSettlementResp GameSettlement;
}

[Serializable, ProtoContract]
public class GameSettlementResp
{
    [ProtoMember(1)]
    public List<GameSettlementInfo> GameSettlementInfo;
}

[Serializable, ProtoContract]
public class GameSettlementInfo
{
    [ProtoMember(1)]
    public int GameID { get; set; }

    [ProtoMember(2)]
    public int UserID { get; set; }

    [ProtoMember(3)]
    public int CurScore { get; set; }

    [ProtoMember(4)]
    public int Score { get; set; }

    /// <summary> 游戏记录数据 </summary>
    [ProtoMember(5)]
    public Dictionary<string, int> GameData { get; set; }

    /// <summary> 胡牌次数 </summary>
    [ProtoMember(6)]
    public int HuNo { get; set; }

    /// <summary> 昵称 </summary>
    [ProtoMember(7)]
    public string Name { get; set; }

    /// <summary> 每局分数 </summary>
    [ProtoMember(8)]
    public List<int> Scores { get; set; }

    /// <summary>
    /// 标记:-1最佳炮手 1大赢家
    /// </summary>
    /// <value>The scores.<alue>
    [ProtoMember(9)]
    public int Flag { get; set; }
}

[Serializable, ProtoContract]
public class SingleSettlementResp
{
    [ProtoMember(1)]
    public List<SingleSettlementInfo> SingleSettlementInfos;

    [ProtoMember(2)]
    public EndType IsWinner { get; set; }

    /// <summary> 翻开的牌 </summary>
    [ProtoMember(3)]
    public List<Card> OpenCards { get; set; }

    /// <summary> 推导出的牌 </summary>
    [ProtoMember(4)]
    public List<Card> OtherCards { get; set; }
}

public enum EndType
{
    LiuJu,
    Victory,
    Fail,
}

[Serializable, ProtoContract]
public class SingleSettlementInfo
{
    [ProtoMember(1)]
    public int UserID { get; set; }

    /// <summary> 昵称 </summary>
    [ProtoMember(2)]
    public string Name { get; set; }

    /// <summary> 是否胡 </summary>
    [ProtoMember(3)]
    public bool IsHu { get; set; }

    /// <summary> 手牌牌组 </summary>
    [ProtoMember(4)]
    public List<CardGroup> HandCardGroups { get; set; }

    /// <summary> 手牌 </summary>
    [ProtoMember(5)]
    public List<Card> HandCards { get; set; }

    /// <summary> 胡的牌 </summary>
    [ProtoMember(6)]
    public Card CurCard { get; set; }

    /// <summary> 分数字典 </summary>
    [ProtoMember(7)]
    public Dictionary<string, int> Scores { get; set; }

    /// <summary> 当前回合总分 </summary>
    [ProtoMember(8)]
    public int TotalScore { get; set; }

    [ProtoMember(9)]
    public string HuFlag { get; set; }

    /// <summary>
    /// 1 点炮的人
    /// </summary>
    [ProtoMember(10)]
    public int Flag { get; set; }

}

public enum RoundEndType
{
    RoundEnd,		
    Disband,		//解散
    GameEnd,
}

#endregion 结算 GameEndActionResp


[Serializable]
public class GameRecordsResp
{
	public List<GameRecordData> GameRecordDatas = new List<GameRecordData>();
}
[Serializable]
public class GameRecordData
{
	public int RecordId; //记录Id
	public string RecordTime; //记录时间
	public string HuDes; //胡的牌型
	public int Flag; // 1是胡 -1是点炮
	public int RecordScore; //当局分数
	public List<CardGroup> HandCardGroups;
	public List<Card> HandCards;
	public Card CurCard;

	public GameRecordData() { }
}
	
[Serializable]
public class GameActionRecorData
{
	public List<GameActionRecord> GameActionList = new List<GameActionRecord>();
	public LiamRoomInfo roomInfo = new LiamRoomInfo();
	public List<LiamUserInfo> userList = new List<LiamUserInfo> ();
	public List<RoundMessageRecord> endList = new List<RoundMessageRecord>();
}

public class LiamRoomInfo{
	public int roomNum;
	public string roomSetting;
}

public class LiamUserInfo{
	public int userId;
	public string userName;
	public int sex;
	public string headUrl;
	public bool isCreater; //是否是房主
}

public class GameActionRecord
{
	/// <summary>
	/// 记录ID
	/// </summary>
	/// <value>The identifier.</value>
	[ProtoMember(1)]
	public int ID { get; set; }

	/// <summary>
	/// 行为
	/// </summary>
	/// <value>The action.</value>
	[ProtoMember(2)]
	public int Action { get; set; }

	/// <summary>
	/// 辅助参数
	/// </summary>
	/// <value>The parame.</value>
	[ProtoMember(3)]
	public int Parame { get; set; }

	/// <summary>
	/// 操作牌
	/// </summary>
	/// <value>The cards.</value>
	[ProtoMember(4)]
	public string Cards { get; set; }

	/// <summary>
	/// 操作用户ID
	/// </summary>
	/// <value>The uid.</value>
	[ProtoMember(5)]
	public int UserID { get; set; }

	/// <summary>
	/// 等待时间
	/// </summary>
	/// <value>The timer.</value>
	[ProtoMember(6)]
	public int Timer { get; set; }

	/// <summary>
	/// 剩余牌张数
	/// </summary>
	/// <value>The cards no.</value>
	[ProtoMember(7)]
	public int CardsNo { get; set; }

	/// <summary>
	/// 起手翻牌或结束时翻牌判断的特殊用的牌
	/// </summary>
	/// <value>The other cards.</value>
	[ProtoMember(8)]
	public string OtherCards { get; set; }

	/// <summary>
	/// 所属回合ID
	/// </summary>
	/// <value>The round identifier.</value>
	[ProtoMember(9)]
	public int RoundID { get; set; }

	/// <summary>
	/// 玩家序号
	/// </summary>
	/// <value>The round identifier.</value>
	[ProtoMember(10)]
	public int Order { get; set; }

	[ProtoMember(11)]
	public string TempTimeModify { get; set; }

	[ProtoMember(12)]
	public string  ExpiredTime { get; set; }

}
	
public class RoundMessageRecord
{
	/// <summary>
	/// Gets or sets the round identifier.
	/// </summary>
	/// <value>The round identifier.</value>
	[ProtoMember(1)]
	public int RoundID { get; set; }

	/// <summary>
	/// Gets or sets the user identifier.
	/// </summary>
	/// <value>The user identifier.</value>
	[ProtoMember(2)]
	public int UserID { get; set; }

	/// <summary>
	/// 昵称
	/// </summary>
	/// <value>The name.</value>
	[ProtoMember(3)]
	public string Name { get; set; }

	/// <summary>
	/// 是否胡
	/// </summary>
	/// <value><c>true</c> if this instance is hu; otherwise, <c>false</c>.</value>
	[ProtoMember(4)]
	public bool IsHu { get; set; }

	/// <summary>
	/// 手牌牌组
	/// </summary>
	/// <value>The hand card groups.</value>
	[ProtoMember(5)]
	public string HandCardGroups { get; set; }

	/// <summary>
	/// 手牌
	/// </summary>
	/// <value>The hand cards.</value>
	[ProtoMember(6)]
	public string HandCards { get; set; }

	/// <summary>
	/// 胡的牌
	/// </summary>
	/// <value>The current card.</value>
	[ProtoMember(7)]
	public string CurCard { get; set; }

	/// <summary>
	/// 分数字典
	/// </summary>
	/// <value>The scores.</value>
	[ProtoMember(8)]
	public string Scores { get; set; }

	/// <summary>
	/// 当前回合总分
	/// </summary>
	/// <value>The total score.</value>
	[ProtoMember(9)]
	public int TotalScore { get; set; }

	/// <summary>
	/// 胡牌标识
	/// </summary>
	/// <value>The hu flag.</value>
	[ProtoMember(10)]
	public string HuFlag { get; set; }

	/// <summary>
	/// Gets or sets the lost user.
	/// </summary>
	/// <value>The lost user.</value>
	[ProtoMember(11)]
	public int Flag { get; set; }

	/// <summary>
	/// 放炮次数
	/// </summary>
	/// <value>The lost user.</value>
	[ProtoMember(12)]
	public int FangPaoNo { get; set; }

	/// <summary>
	/// 时间
	/// </summary>
	/// <value>The time.</value>
	[ProtoMember(13)]
	public string Time { get; set; }

	[ProtoMember(14)]
	public string TempTimeModify { get; set; }

	[ProtoMember(15)]
	public string  ExpiredTime { get; set; }
}








