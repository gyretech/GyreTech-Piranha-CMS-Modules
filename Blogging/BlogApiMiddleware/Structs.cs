using System;
using System.Collections.Generic;
using System.Text;
using CookComputing.XmlRpc;

namespace BlogApiMiddleware
{
    public struct BlogInfo
    {
        public string blogid;
        public string url;
        public string blogName;
        public bool isAdmin;
    }

    public struct Category
    {
        public string categoryId;
        public string categoryName;
        public string categoryDescription;
    }

    [Serializable]
    public struct CategoryInfo
    {
        public string description;
        public string htmlUrl;
        public string title;
        public string categoryid;
        public string categoryName;
        public string categoryDescription;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Enclosure
    {
        public int length;
        public string type;
        public string url;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Post
    {
        public object postid;
        public string title;
        public string description;
        public string userid;
        public DateTime dateCreated;
        public object date_created_gmt;
        public object date_modified;
        public object date_modified_gmt;
        public string wp_post_thumbnail;
        public string mt_text_more;
        public string[] categories;
        public string mt_keywords;
        public string permalink;


        public string wp_slug;

    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct PostContent
    {
        public DateTime dateCreated;
        public string description;
        public string title;
        public string[] categories;
        public string permalink;
        public object postid;
        public string userid;
        public string wp_slug;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct Source
    {
        public string name;
        public string url;
    }

    public struct UserInfo
    {
        public string userid;
        public string firstname;
        public string lastname;
        public string nickname;
        public string email;
        public string url;
    }

    [XmlRpcMissingMapping(MappingAction.Ignore)]
    public struct MediaObject
    {
        public string name;
        public string type;
        public byte[] bits;
    }

    [Serializable]
    public struct MediaObjectInfo
    {
        public string url;
    }
}
