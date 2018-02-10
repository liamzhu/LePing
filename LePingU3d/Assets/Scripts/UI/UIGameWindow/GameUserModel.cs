using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUserModel
{
    private List<UserInfo> mUsers;

    public GameUserModel()
    {
        mUsers = new List<UserInfo>();
    }

    public GameUserModel(UserInfo user)
    {
        mUsers = new List<UserInfo>();
        mUsers.Add(user);
    }

    public GameUserModel(List<UserInfo> users)
    {
        mUsers = new List<UserInfo>(users);
    }

    public void addUser(UserInfo user)
    {
        int index = getIndex(user.UserId);
        if (index != -1)
        {
            mUsers[index] = user;
        }
        else
        {
            mUsers.Add(user);
        }
    }

    public void removeUser(UserInfo user)
    {
        mUsers.Remove(user);
    }

    public void removeUser(int id)
    {
        mUsers.RemoveAll(p => p.UserId == id);
    }

    private int getIndex(int id)
    {
        return mUsers.FindIndex(p => p.UserId == id);
    }

    public List<UserInfo> getUsers()
    {
        return mUsers;
    }

    public UserInfo getUser(int userid)
    {
        if (mUsers != null)
        {
            UserInfo user = mUsers.Find(p => p.UserId == userid);
            if (user != null)
            {
                return user;
            }
            return null;
        }
        else { return null; }
    }

    public void CleanUp()
    {
        mUsers = new List<UserInfo>();
    }
}
