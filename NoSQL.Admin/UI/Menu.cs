using NoSQL.DAL.Data;
using NoSQL.DTO.Models;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using NoSQL.DAL.Repositories.Concreate.DataBaseMongoDBNoSQL;
using NoSQL.DAL.Repositories.Abstract.DataBaseMongoDBNoSQL;
using NoSQL.BLL.Repositories.Concreate.DataBaseMongoDBNoSQL;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Runtime.InteropServices;

namespace NoSQL.Admin.UI
{
    class Menu
    {
        private Driver _driver;
        private PostsRepositoryBLL _postsRepository;
        private UsersRepositoryBLL _usersRepository;
        private string _email;
        private string _password;
        private string _ownerId;

        public Menu()
        {
            _driver = new Driver(1);
            _postsRepository = new PostsRepositoryBLL(_driver);
            _usersRepository = new UsersRepositoryBLL(_driver);

            while (Authentication()) { }
        }

        public void Demo()
        {
            while (DemoOnce()) { }
        }

        private bool DemoOnce()
        {
            Console.WriteLine("Select option:\n1. - Print All Posts.\n2. - Create Post.\n3. - Replace Post." +
                "\n4. - Delete Post.\n5. - Create Comment.\n6. - Print All Comments from one post.\n7. - Like post.\n8. - UnLike post" +
                "\n9. - Like Comment.\n10. - UnLike Comment.\n11. - Add Friend.\n12. - Delete Friend.\n13. - Print All posts from One user\n0. - Login Menu.\n-1 - Exit");
            string userInput = Console.ReadLine();

            try
            {
                switch (userInput)
                {
                    case "1":
                        PrintAllPosts();
                        return true;
                    case "2":
                        CreatePost();
                        return true;
                    case "3":
                        ReplacePost();
                        return true;
                    case "4":
                        DeletePost();
                        return true;
                    case "5":
                        CreateComment();
                        return true;
                    case "6":
                        PrintAllCommentsFromOnePost();
                        return true;
                    case "7":
                        LikePost();
                        return true;
                    case "8":
                        UnLikePost();
                        return true;
                    case "9":
                        LikeComment();
                        return true;
                    case "10":
                        UnLikeComment();
                        return true;
                    case "11":
                        AddFriend();
                        return true;
                    case "12":
                        DeleteFriend();
                        return true;
                    case "13":
                        PrintAllPostsFromOneUser();
                        return true;
                    case "0":
                        while (Authentication()) { }
                        return true;
                    case "-1":
                        return false;
                    default:
                        return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured, check your input data");
                Console.WriteLine();
                return true;
            }
        }

        public bool Authentication()
        {
            _email = "";
            _password = "";
            Console.WriteLine("Select option:\n1. - Sign in.\n0. - Sign up\n-1. - Exit.");
            string userInput = Console.ReadLine();
            try
            {
                switch (userInput)
                {
                    case "1":
                        Console.WriteLine("~~~~~Enter Email and Password~~~~~");

                        var userInputList = Console.ReadLine()!.Split(' ');
                        _email = userInputList[0];
                        _password = userInputList[1];
                        return IsAuthenticated(_email, _password);
                    case "0":
                        Console.WriteLine("~~~~~Enter Email, First Name, Last Name and Password~~~~~");

                        var userInputList0 = Console.ReadLine()!.Split(' ');
                        _email = userInputList0[0];
                        var firstname = userInputList0[1];
                        var lastname = userInputList0[2];
                        _password = userInputList0[3];
                        return IsRegistered(_email, firstname, lastname, _password);
                    case "-1":
                        Console.WriteLine("~~~~~Access terminated~~~~~");
                        Environment.Exit(0);
                        return false;
                    default:
                        return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occured:");
                Console.WriteLine(ex);
                return true;
            }
        }

        public bool IsRegistered(string email, string firstname, string lastname, string password)
        {
            try
            {
                var user = _usersRepository.GetCollection().Find(p => p.Email == email).Single();
            }
            catch (Exception ex) 
            {
                var newUser = new User
                {
                    Email = email,
                    FirstName = firstname,
                    LastName = lastname,
                    Password = password,
                    Interests = new List<string>(),
                    Friends = new List<string>()
                };
                _usersRepository.Create(newUser);
                _ownerId = newUser.Id;

                Console.WriteLine($"~~~~~Access granted~~~~~" +
                            $"\n~~~~~Welcome {newUser.FirstName} {newUser.LastName}~~~~~");
                PrintAllSortedPosts();
                return false;
            }
            Console.WriteLine("~~~~~Access denied~~~~~" +
                        "\n~~~~~This account is already exist~~~~~");
            return true;
        }

        public bool IsAuthenticated(string email, string password)
        {
            User user;
            try 
            {
                user = _usersRepository.GetCollection().Find(p => p.Email == email).Single();
            }
            catch(Exception ex)
            {
                Console.WriteLine("~~~~~Access denied~~~~~" +
                        "\n~~~~~This account is not exist~~~~~");
                return true;
            }
            if (email == user.Email)
            {
                if (password == user.Password)
                {
                    _ownerId = user.Id;
                    Console.WriteLine($"~~~~~Access granted~~~~~" +
                        $"\n~~~~~Welcome {user.FirstName} {user.LastName}~~~~~");
                    PrintAllSortedPosts();
                    return false;
                }
                else
                {
                    Console.WriteLine("~~~~~Access denied~~~~~" +
                        "\n~~~~~Wrong Password~~~~~");
                    return true;
                }
            }
            else 
            {
                Console.WriteLine("~~~~~Access denied~~~~~" +
                "\n~~~~~Wrong Email~~~~~");
                return true;
            }
        }

        public void PrintAllSortedPosts()
        {
            var list = _postsRepository.GetCollection().Find(p => true).ToList().OrderByDescending(p => p.PostCreatedDate).ToList();
            foreach (var post in list)
            {
                var ownerUser = _usersRepository.GetCollection().Find(p => p.Id == post.OwnerId).Single();
                Console.WriteLine($"PostId: {post.Id}\nTitle: {post.Title}\nBody: {post.PostBody}\nCategory: {post.Category}, Date: {post.PostCreatedDate}, Likes: {post.Like.Likes}\n" +
                    $"Owner: {ownerUser.FirstName} {ownerUser.LastName}\n");
            }
        }

        public void PrintAllPosts()
        {
            var list = _postsRepository.GetCollection().Find(p => true).ToList();
            foreach (var post in list) 
            {
                var ownerUser = _usersRepository.GetCollection().Find(p => p.Id == post.OwnerId).Single();
                Console.WriteLine($"PostId: {post.Id}\nTitle: {post.Title}\nBody: {post.PostBody}\nCategory: {post.Category}, Date: {post.PostCreatedDate}, Likes: {post.Like.Likes}\n" +
                    $"Owner: {ownerUser.FirstName} {ownerUser.LastName}\n");
            }
        }

        public void CreatePost()
        {
            Console.WriteLine("Enter title: ");
            string title = Console.ReadLine();
            Console.WriteLine("Enter body:");
            string postBody = Console.ReadLine();
            Console.WriteLine("Enter category:");
            string category = Console.ReadLine();

            Post newPost = new Post
            {
                Title = title,
                PostBody = postBody,
                Category = category,
                PostCreatedDate = DateTime.Now,
                Like = new Like 
                {
                    Likes = 0,
                    UsersIdLiked = new List<string>()
                },
                OwnerId = _ownerId,
                Comments = new List<Comment>()
            };
            _postsRepository.Create(newPost);
            Console.WriteLine("Post Added");
        }

        public void ReplacePost()
        {
            Console.WriteLine("Enter Post Id: ");
            string postId = Console.ReadLine();
            try
            {
                var existingPost = _postsRepository.GetCollection().Find(p => p.Id == postId).Single();
                if (existingPost.OwnerId == _ownerId)
                {
                    Console.WriteLine("Enter title: ");
                    string title = Console.ReadLine();
                    Console.WriteLine("Enter body:");
                    string postBody = Console.ReadLine();
                    Console.WriteLine("Enter category:");
                    string category = Console.ReadLine();
                    Post newPost = new Post
                    {
                        Id = postId,
                        Title = title,
                        PostBody = postBody,
                        Category = category,
                        PostCreatedDate = DateTime.Now,
                        Like = new Like
                        {
                            Likes = 0,
                            UsersIdLiked = new List<string>()
                        },
                        OwnerId = _ownerId,
                        Comments = new List<Comment>()
                    };
                    _postsRepository.Replace(newPost);

                    Console.WriteLine("Post Replaced");
                }
                else
                {
                    Console.WriteLine("This post was not created by you\nCommand denied");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("This post is not exist");
            }
        }

        public void DeletePost()
        {
            Console.WriteLine("Enter Post Id");
            var postId = Console.ReadLine();
            try
            {
                var existingPost = _postsRepository.GetCollection().Find(p => p.Id == postId).Single();
                if (existingPost.OwnerId == _ownerId) 
                {
                    Post newPost = new Post
                    {
                        Id = postId
                    };
                    _postsRepository.Delete(newPost);
                    Console.WriteLine("Post Deleted");
                }
                else
                {
                    Console.WriteLine("This post was not created by you\nCommand denied");
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine("This post is not exist");
            }
        }

        public void CreateComment() 
        {
            Console.WriteLine("Enter Post Id");
            var postId = Console.ReadLine();
            try
            {
                var existingPost = _postsRepository.GetCollection().Find(p => p.Id == postId).Single();
                Console.WriteLine("Enter body: ");
                string commentBody = Console.ReadLine();
                Comment newComment = new Comment
                {
                    CommentBody = commentBody,
                    OwnerId = _ownerId,
                    CommentCreatedDate = DateTime.Now,
                    Like = new Like
                    {
                        Likes = 0,
                        UsersIdLiked = new List<string>()
                    }
                };
                _postsRepository.CreateComment(newComment, postId);
                Console.WriteLine("Comment Added");
            }
            catch (Exception ex)
            {
                Console.WriteLine("This post is not exist");
            }
        }
        
        public void PrintAllCommentsFromOnePost()
        {
            Console.WriteLine("Enter Post Id");
            var postId = Console.ReadLine();
            try
            {
                var existingPost = _postsRepository.GetCollection().Find(p => p.Id == postId).Single();
                User user;
                foreach (var comment in existingPost.Comments) 
                {
                    user = _usersRepository.GetCollection().Find(p => p.Id == comment.OwnerId).Single();
                    Console.WriteLine($"Comment id: {comment.Id}\nBody: {comment.CommentBody}\nDate: {comment.CommentCreatedDate}, Likes: {comment.Like.Likes}\n" +
                        $"Owner: {user.FirstName} {user.LastName}\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("This post is not exist");
            }
        }

        public void LikePost()
        {
            Console.WriteLine("Enter Post Id");
            var postId = Console.ReadLine();
            try
            {
                var existingPost = _postsRepository.GetCollection().Find(p => p.Id == postId).Single();
                if (!existingPost.Like.UsersIdLiked.Contains(_ownerId))
                {
                    _postsRepository.LikePost(existingPost.Id, _ownerId);
                    Console.WriteLine("Post Liked");
                }
                else
                {
                    Console.WriteLine("This post was already liked by you\nCommand denied");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("This post is not exist");
            }
        }

        public void LikeComment()
        {
            Console.WriteLine("Enter Post Id");
            var postId = Console.ReadLine();
            try
            {
                var existingPost = _postsRepository.GetCollection().Find(p => p.Id == postId).Single();
                try
                {
                    Console.WriteLine("Enter Comment Id");
                    var commentId = Console.ReadLine();
                    var commentsList = existingPost.Comments;
                    var existingComment = commentsList.Find(p => p.Id == commentId);
                    if (!existingComment.Like.UsersIdLiked.Contains(_ownerId) && existingComment != null)
                    {
                        _postsRepository.LikeComment(existingPost.Id, commentId, _ownerId);
                        Console.WriteLine("Comment Liked");
                    }
                    else if (existingComment == null)
                    {
                        Console.WriteLine("This comment is not exist");
                    }
                    else
                    {
                        Console.WriteLine("This comment was already liked by you\nCommand denied");
                    }
                }
                catch (Exception ex) 
                {
                    Console.WriteLine("This comment is not exist");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("This post is not exist");
            }
        }

        public void AddFriend()
        {
            Console.WriteLine("Enter email");
            var email = Console.ReadLine();
            if (email == _email)
            {
                Console.WriteLine("You can't add yourself");
                return;
            }
            try
            {
                var existingUser = _usersRepository.GetCollection().Find(p => p.Email == email).Single();

                if (existingUser.Friends.Contains(existingUser.Id)) 
                {
                    Console.WriteLine("This user was already added by you\nCommand denied");
                }
                else
                {
                    _usersRepository.AddFriend(_ownerId, existingUser.Id);
                    Console.WriteLine("Friend Added");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("This user is not exist");
            }
        }

        public void DeleteFriend()
        {
            Console.WriteLine("Enter email");
            var email = Console.ReadLine();
            if (email == _email)
            {
                Console.WriteLine("You can't delete yourself");
                return;
            }
            try
            {
                var existingUser = _usersRepository.GetCollection().Find(p => p.Email == email).Single();
                var myuser = _usersRepository.GetCollection().Find(p => p.Email == _email).Single();

                if (myuser.Friends.Contains(existingUser.Id))
                {
                    _usersRepository.DeleteFriend(_ownerId, existingUser.Id);
                    Console.WriteLine("Friend Deleted");
                }
                else
                {
                    Console.WriteLine("This user was already deleted or was not added by you\nCommand denied");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("This user is not exist");
            }
        }

        public void PrintAllPostsFromOneUser()
        {
            Console.WriteLine("Enter email");
            var email = Console.ReadLine();
            try
            {
                var existingUser = _usersRepository.GetCollection().Find(p => p.Email == email).Single();
                var list = _postsRepository.GetCollection().Find(p => true).ToList();
                foreach (var post in list)
                {
                    if (post.OwnerId == existingUser.Id)
                    {
                        var ownerUser = _usersRepository.GetCollection().Find(p => p.Id == post.OwnerId).Single();
                        Console.WriteLine($"PostId: {post.Id}\nTitle: {post.Title}\nBody: {post.PostBody}\nCategory: {post.Category}, Date: {post.PostCreatedDate}, Likes: {post.Like.Likes}\n" +
                            $"Owner: {ownerUser.FirstName} {ownerUser.LastName}\n");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("This user is not exist");
            }
        }

        public void UnLikePost()
        {
            Console.WriteLine("Enter Post Id");
            var postId = Console.ReadLine();
            try
            {
                var existingPost = _postsRepository.GetCollection().Find(p => p.Id == postId).Single();
                if (existingPost.Like.UsersIdLiked.Contains(_ownerId))
                {
                    _postsRepository.UnLikePost(existingPost.Id, _ownerId);
                    Console.WriteLine("Post UnLiked");
                }
                else
                {
                    Console.WriteLine("This post was already unliked by you\nCommand denied");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("This post is not exist");
            }
        }

        public void UnLikeComment()
        {
            Console.WriteLine("Enter Post Id");
            var postId = Console.ReadLine();
            try
            {
                var existingPost = _postsRepository.GetCollection().Find(p => p.Id == postId).Single();
                try
                {
                    Console.WriteLine("Enter Comment Id");
                    var commentId = Console.ReadLine();
                    var commentsList = existingPost.Comments;
                    var existingComment = commentsList.Find(p => p.Id == commentId);
                    if (existingComment.Like.UsersIdLiked.Contains(_ownerId) && existingComment != null)
                    {
                        _postsRepository.UnLikeComment(existingPost.Id, commentId, _ownerId);
                        Console.WriteLine("Comment UnLiked");
                    }
                    else if (existingComment == null)
                    {
                        Console.WriteLine("This comment is not exist");
                    }
                    else
                    {
                        Console.WriteLine("This comment was already unliked by you\nCommand denied");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("This comment is not exist");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("This post is not exist");
            }
        }
    }
}