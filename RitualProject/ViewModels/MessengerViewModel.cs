
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RitualServer.Model;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Message = RitualServer.Model.Message;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using Newtonsoft.Json;
using YandexDisk.Client.Http;
using System.Diagnostics;
using YandexDisk.Client.Clients;

namespace RitualProject
{
    public class MessengerViewModel : ViewModel
    {
        private readonly ApiClient _apiClient = new ApiClient();
        #region Properties
        public int id { get; set; }

        private bool flag;

        private string _login;
        public string Login
        {
            get { return _login; }
            set => Set(ref _login, value);
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set => Set(ref _email, value);

        }
        private byte[] _image = null;
        public byte[] Image
        {
            get { return _image; }
            set => Set(ref _image, value);
        }

        public string LastSearchTxt { get; set; }
        private string _TxtEntered;
        public string TxtEntered
        {
            get { return _TxtEntered; }
            set
            {
                _TxtEntered = value;
                OnPropertyChanged("TxtEntered");
                Search();
            }
        }

        public string LastSeartchMessageText { get; set; }
        private string _SearchMessageText;
        public string SearchMessageText
        {
            get { return _SearchMessageText; }
            set
            {
                _SearchMessageText = value;
                OnPropertyChanged("SearchMessageText");
                SearchMessage();
            }
        }

        private string _messageText;
        public string MessageText
        {
            get { return _messageText; }
            set => Set(ref _messageText, value);
        }

        private bool _isSearchBoxOpen = false;
        public bool IsSearchBoxOpen
        {
            get { return _isSearchBoxOpen; }
            set
            {
                _isSearchBoxOpen = value;
                if (_isSearchBoxOpen == false)
                {
                    TxtEntered = string.Empty;
                }
                OnPropertyChanged("IsSearchBoxOpen");
            }
        }
        private bool _isMessageSearchBoxOpen = false;
        public bool IsMessageSearchBoxOpen
        {
            get { return _isMessageSearchBoxOpen; }
            set
            {
                _isMessageSearchBoxOpen = value;
                if (_isMessageSearchBoxOpen == false)
                {
                    SearchMessageText = string.Empty;
                }
                OnPropertyChanged("IsMessageSearchBoxOpen");
            }
        }
        private bool _isContactInfoOpen = false;
        public bool IsContactInfoOpen
        {
            get { return _isContactInfoOpen; }
            set => Set(ref _isContactInfoOpen, value);
        }


        private bool _focusMessageBox;
        public bool FocusMessageBox
        {
            get { return _focusMessageBox; }
            set => Set(ref _focusMessageBox, value);
        }
        private bool _isThisReplyMessage;
        public bool IsThisReplyMessage
        {
            get { return _isThisReplyMessage; }
            set => Set(ref _isThisReplyMessage, value);
        }
        private string _messageToReplyText = string.Empty;
        public string MessageToReplyText
        {
            get { return _messageToReplyText; }
            set => Set(ref _messageToReplyText, value);
        }
        private User userselected;
        public User UserSelected
        {
            get { return userselected; }
            set => Set(ref userselected, value);
        }
        private Participant participantSelected;
        public Participant ParticipantSelected
        {
            get { return participantSelected; }
            set => Set(ref participantSelected, value);
        }
        private List<User> _usersList;
        public List<User> UsersList
        {
            get { return _usersList; }
            set => Set(ref _usersList, value);
        }

        private List<Conservation> _razgovorList;
        public List<Conservation> RazgovorList
        {
            get { return _razgovorList; }
            set => Set(ref _razgovorList, value);
        }

        private List<Participant> _participantsList;
        public List<Participant> ParticipantsList
        {
            get { return _participantsList; }
            set => Set(ref _participantsList, value);
        }

        private ObservableCollection<Message> _messagesList;
        public ObservableCollection<Message> MessagesList
        {
            get { return _messagesList; }
            set => Set(ref _messagesList, value);
        }
        private ObservableCollection<Message> _messagesColection;
        public ObservableCollection<Message> MessagesCollection
        {
            get { return _messagesColection; }
            set => Set(ref _messagesColection, value);
        }
        //Все сообщения
        private ObservableCollection<MessagesListMy> _messagesListMy;
        public ObservableCollection<MessagesListMy> MessagesListMy
        {
            get { return _messagesListMy; }
            set => Set(ref _messagesListMy, value);
        }

        //Сообщения на выбранном разговоре
        private ObservableCollection<MessagesListMy> _messagesListNow;
        public ObservableCollection<MessagesListMy> MessagesListNow
        {
            get { return _messagesListNow; }
            set => Set(ref _messagesListNow, value);
        }

        private List<ConservationsMy> _chats;
        public List<ConservationsMy> Chats
        {
            get { return _chats; }
            set => Set(ref _chats, value);

        }
        private ObservableCollection<Conservation> _razgovor;
        public ObservableCollection<Conservation> Razgovor
        {
            get { return _razgovor; }
            set => Set(ref _razgovor, value);

        }
        //Закрепленные чаты
        private ObservableCollection<ConservationsMy> _pinnedConservations;
        public ObservableCollection<ConservationsMy> PinnedConservations
        {
            get { return _pinnedConservations; }
            set => Set(ref _pinnedConservations, value);

        }
        private ObservableCollection<ConservationsMy> _filteredConservations;
        public ObservableCollection<ConservationsMy> FilteredConservations
        {
            get { return _filteredConservations; }
            set => Set(ref _filteredConservations, value);

        }
        private ObservableCollection<ConservationsMy> _conservationsMies;
        public ObservableCollection<ConservationsMy> ConservationsMies
        {
            get { return _conservationsMies; }
            set => Set(ref _conservationsMies, value);
        }
        public ObservableCollection<ConservationsMy> FilteredPinnedConservations { get; set; }
        private ObservableCollection<ConservationsMy> _archivedConservations;
        public ObservableCollection<ConservationsMy> ArchivedConservations
        {
            get { return _archivedConservations; }
            set => Set(ref _archivedConservations, value);
        }
        private ObservableCollection<VlozheniaMess> _vlozheniaMess;
        public ObservableCollection<VlozheniaMess> VlozheniaMess
        {
            get { return _vlozheniaMess; }
            set => Set(ref _vlozheniaMess, value);
        }
        private ConservationsMy selectedCons;
        public ConservationsMy SelectedCons
        {
            get { return selectedCons; }
            set
            {
                //BooksViewModel book=value;
                selectedCons = value;

                if (selectedCons != null)
                {
                    MessagesListMy.Clear();
                    MessagesListNow.Clear();
                    SearchMessageText = "";
                    Login = selectedCons.Login;
                    Image = selectedCons.Image;
                    bool flag;
                    id = selectedCons.Id;
                    MessagesTake();
                   
                    foreach (Message item in MessagesCollection)
                    {
                        if (item.RazgovorId == id)
                        {
                            flag = false;

                            if (item.SenderId == UserInfoConstant.ID)
                            {
                                flag = true;
                            }
                            var vlozhenia = item.VlozheniaMesses.Where(x => x.MessageId == item.MessageId).FirstOrDefault();
                            byte[] image = null;
                            string fileURL = null;
                            if (vlozhenia != null)
                            {
                                filename = vlozhenia.FileUrl;
                                image = vlozhenia.ImagePho;
                            }
                            
                            var Messages = new MessagesListMy
                            {
                                Id = item.MessageId,
                                ImagePho = image,
                                Message = item.Message1,
                                Created_at = item.CreatedAt,
                                //Deleted_at = item.Deleted_at,
                                Razgovor_id = item.RazgovorId,
                                Sender_id = item.SenderId,
                                FileURL = filename,
                                //Type_id = item.Type_id,
                                IsSender = flag
                            };
                            MessagesListMy.Add(Messages);
                            MessagesListNow.Add(Messages);
                        }
                    }
                }
                OnPropertyChanged("SelectedCons");
            }
        }
        #endregion
        #region Methods

        public void Search()
        {
            if ((string.IsNullOrEmpty(TxtEntered) && string.IsNullOrEmpty(LastSearchTxt)) || string.Equals(LastSearchTxt, TxtEntered))
            {
                return;
            }
            if (string.IsNullOrEmpty(TxtEntered) || ConservationsMies == null || ConservationsMies.Count <= 0)
            {
                FilteredConservations = new ObservableCollection<ConservationsMy>(ConservationsMies ?? Enumerable.Empty<ConservationsMy>());
                OnPropertyChanged("FilteredConservations");
                FilteredPinnedConservations = new ObservableCollection<ConservationsMy>(PinnedConservations ?? Enumerable.Empty<ConservationsMy>());
                OnPropertyChanged("FilteredPinnedConservations");

                LastSearchTxt = TxtEntered;
                return;
            }
            FilteredConservations = new ObservableCollection<ConservationsMy>(ConservationsMies.Where(chat => chat.Login.ToLower().Contains(TxtEntered)));
            OnPropertyChanged("FilteredConservations");
            FilteredPinnedConservations = new ObservableCollection<ConservationsMy>(PinnedConservations.Where(pinnedchat => pinnedchat.Login.ToLower().Contains(TxtEntered)));
            OnPropertyChanged("FilteredPinnedConservations");
            LastSearchTxt = TxtEntered;
        }

        public void SearchMessage()
        {
            if ((string.IsNullOrEmpty(SearchMessageText) && string.IsNullOrEmpty(LastSeartchMessageText)) || string.Equals(LastSeartchMessageText, SearchMessageText))
            {
                return;
            }
            if (string.IsNullOrEmpty(SearchMessageText) || ConservationsMies == null || ConservationsMies.Count <= 0)
            {
                MessagesListNow = new ObservableCollection<MessagesListMy>(MessagesListMy ?? Enumerable.Empty<MessagesListMy>());
                //OnPropertyChanged("FilteredConservations");
                LastSeartchMessageText = SearchMessageText;
                return;
            }
            MessagesListNow = new ObservableCollection<MessagesListMy>(MessagesListMy.Where(chat => chat.Message.ToLower().Contains(SearchMessageText)));
            //OnPropertyChanged("FilteredConservations");

            LastSeartchMessageText = SearchMessageText;
        }
        public void SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(MessageText))
            {
                try
                {
                    int count = MessagesListNow.Count;

                    MessagesListMy cons = new MessagesListMy
                    {
                        Id = ++count,
                        ImagePho = null,
                        Message = MessageText,
                        Created_at = DateTime.Now,
                        Deleted_at = null,
                        Sender_id = UserInfoConstant.ID,
                        Type_id = null,
                        IsSender = true,
                        Razgovor_id = SelectedCons.Id
                    };
                    MessagesListNow.Add(cons);
                    MessagesListMy.Add(cons);
                    MessageText = string.Empty;
                    IsThisReplyMessage = false;
                    MessageToReplyText = string.Empty;
                    PostMessageAsync(cons);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                //Добавить сюда добавление в БД
            }
        }
        #endregion
        #region Constructor
        public async Task FindUsers()
        {
            var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getArticle");
            response.EnsureSuccessStatusCode();
            var userArray = await response.Content.ReadFromJsonAsync<User[]>();
            UsersList = new List<User>(userArray);
            
        }
        public async Task FindConserv()
        {
            var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getConservation");
            response.EnsureSuccessStatusCode();
            var conservationArray = await response.Content.ReadFromJsonAsync<Conservation[]>();
           
            var conserv = conservationArray.Where(c => c.Participants.Any(p => p.UsersId == UserInfoConstant.ID))
                               .Select(c => new
                               {
                                   Conservation = c,
                                   Users = c.Participants.FirstOrDefault(p => p.UsersId == UserInfoConstant.ID)?.Users
                               })
                               .ToList();
            foreach(var item in conservationArray)
            {
                var participants = item.Participants.FirstOrDefault(p => p.UsersId != UserInfoConstant.ID);
                var participantsUser = item.Participants.FirstOrDefault(p => p.UsersId == UserInfoConstant.ID);
                if (participantsUser == null)
                {
                    return;
                }
                var lastMessage = MessagesCollection.Where(m => m.RazgovorId == item.ConservationId).OrderByDescending(m => m.MessageId).FirstOrDefault();
                var users = UsersList.FirstOrDefault(p => p.UserId == participants.UsersId);
                string lastMessageText = "Отправьте сообщение";
                int lastMessageId = 0;  
                Nullable<System.DateTime> dateTime = null;
                if (lastMessage != null)
                {
                    lastMessageText = lastMessage.Message1;
                    lastMessageId = lastMessage.MessageId;
                    dateTime = lastMessage.CreatedAt;
                }
                var conservation = new ConservationsMy
                {
                    Id = item.ConservationId,
                    Title = item.Title,
                    Created_at = item.CreatedAt,
                   
                    Creator_id = item.CreatorId,
                    lastMessageId = lastMessageId,
                    Login = users.Login,
                    Image = users.Image,
                    LastMessage = lastMessageText,
                    DateOfLastMessage = dateTime,
                    Is_Pinned = participantsUser.IsPinned,
                    Is_Archived = participantsUser.IsArchived
                };
                ConservationsMies.Add(conservation);
            }
        }
        public async Task Participants()
        {
            var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getParticipants");
            response.EnsureSuccessStatusCode();
            var participantArray = await response.Content.ReadFromJsonAsync<Participant[]>();
            foreach(Participant participants in participantArray)
            {
                if (participants.Conservation.ConservationId == participants.ConservationId)
                {
                    ParticipantsList.Add(participants);
                }
            }

        }
        public async Task FindParticipant(ConservationsMy conservations)
        {
            var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getParticipants");
            response.EnsureSuccessStatusCode();
            var participantArray = await response.Content.ReadFromJsonAsync<Participant[]>();
            foreach (Participant participants in participantArray)
            {
                if (participants.ConservationId == conservations.Id && participants.UsersId==UserInfoConstant.ID)
                {
                    ParticipantSelected= participants;
                }
            }
        }
        public async Task UpdateParticipant(bool flag,bool isarchived)
        {
            try
            {
                if (isarchived == true)
                {
                    ParticipantSelected.IsArchived = flag;
                    var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Participant/{ParticipantSelected.Id}", new StringContent(JsonConvert.SerializeObject(ParticipantSelected), Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    ParticipantSelected.IsPinned = flag;
                    Participant participant = ParticipantSelected;
                    participant.Users = null;
                    participant.Conservation = null;
                    //Participant participant = new Participant()
                    //{
                    //    Id = ParticipantSelected.Id,
                    //    ConservationId = ParticipantSelected.ConservationId,
                    //    UsersId = ParticipantSelected.UsersId,
                    //    IsPinned = ParticipantSelected.IsPinned,
                    //    IsArchived = ParticipantSelected.IsArchived
                    //};
                    var response = await _apiClient.Client.PutAsync($"{_apiClient.BaseUrl}/api/Participant/{ParticipantSelected.Id}", new StringContent(JsonConvert.SerializeObject(ParticipantSelected), Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления: {ex.Message}");
            }
        }
        public async Task Messages()
        {
            var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getMessages");
            response.EnsureSuccessStatusCode();
            var messagesArray = await response.Content.ReadFromJsonAsync<Message[]>();
            foreach (RitualServer.Model.Message item in messagesArray)
            {
                if (item.RazgovorId == id)
                {
                    MessagesList.Add(item);
                    flag = false;
                    if (item.SenderId == UserInfoConstant.ID)
                    {
                        flag = true;
                    }
                    var Messages = new MessagesListMy
                    {
                        Id = item.MessageId,
                        Message = item.Message1,
                        Created_at = item.CreatedAt,
                        Razgovor_id = item.RazgovorId,
                        Sender_id = item.SenderId,
                       // Type_id = item.,
                        IsSender = flag
                    };
                    MessagesListMy.Add(Messages);
                }
            }

        }
        public async Task MessagesTake()
        {
            var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getMessages");
            response.EnsureSuccessStatusCode();
            var messagesArray = await response.Content.ReadFromJsonAsync<Message[]>();
            MessagesCollection = new ObservableCollection<Message>(messagesArray);

        }
        public async Task Conservations()
        {
            var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getConservation");
            response.EnsureSuccessStatusCode();
            var conservationArray = await response.Content.ReadFromJsonAsync<Conservation[]>();
            foreach (Conservation razgovor in conservationArray)
            {
                foreach (Participant participants in ParticipantsList)
                {
                    if (participants.UsersId == UserInfoConstant.ID && razgovor.ConservationId == participants.UsersId)
                    {
                        RazgovorList.Add(razgovor);
                    }
                }
            }
        }
        public async Task ParticipantNow()
        {
            var response = await _apiClient.Client.GetAsync($"{_apiClient.BaseUrl}/getParticipants");
            response.EnsureSuccessStatusCode();
            var participantArray = await response.Content.ReadFromJsonAsync<Participant[]>();
            foreach (Participant participants in participantArray)
            {
                foreach (Conservation razgovors in RazgovorList)
                {
                    if (participants.ConservationId == razgovors.ConservationId)
                    {
                        ParticipantsList.Add(participants);
                    }
                }
            }
            FilteredConservations = new ObservableCollection<ConservationsMy>(ConservationsMies.OrderByDescending(x => x.DateOfLastMessage));
        }
        public async Task PostMessageAsync(MessagesListMy messagesListMy)
        {
            try
            {
                Message message = new Message()
                {
                    Message1 = messagesListMy.Message,
                    CreatedAt = messagesListMy.Created_at,
                    RazgovorId = messagesListMy.Razgovor_id,
                    SenderId=messagesListMy.Sender_id
                };
                var response = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/Message", new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                if (messagesListMy.ImagePho!=null || messagesListMy.FileURL!=null)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var createdMessage = JsonConvert.DeserializeObject<Message>(responseBody);
                    VlozheniaMess vlozhenia = new VlozheniaMess()
                    {
                        ImagePho = messagesListMy.ImagePho,
                        FileUrl=messagesListMy.FileURL,
                        MessageId=createdMessage.MessageId
                    };
                    var responseVlozhenia = await _apiClient.Client.PostAsync($"{_apiClient.BaseUrl}/api/VlozheniaMess", new StringContent(JsonConvert.SerializeObject(vlozhenia), Encoding.UTF8, "application/json"));
                    responseVlozhenia.EnsureSuccessStatusCode();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private async Task InitializeAsync()
        {
            FilteredPinnedConservations = new ObservableCollection<ConservationsMy>();
            PinnedConservations = new ObservableCollection<ConservationsMy>();
            ConservationsMies = new ObservableCollection<ConservationsMy>();
            ArchivedConservations = new ObservableCollection<ConservationsMy>();

            MessagesListMy = new ObservableCollection<MessagesListMy>();
            MessagesListNow = new ObservableCollection<MessagesListMy>();
            RazgovorList = new List<Conservation>();
            MessagesList = new ObservableCollection<Message>();
            ParticipantsList = new List<Participant>();
            ParticipantSelected = new Participant();
            await MessagesTake();
            await FindUsers();
            await FindConserv();
            await Participants();
            await Conservations();
            await Messages();
            await ParticipantNow();
           // Image = ConservationsMies[0].Image;
        }
        public MessengerViewModel()
        {
            InitializeAsync();
        }
        #endregion
        #region Commands
        private RelayCommands _openContactInfo;

        public RelayCommands OpenContactInfo
        {
            get
            {
                return _openContactInfo ?? (_openContactInfo = new RelayCommands(async obj =>
                {
                    if (IsContactInfoOpen == false)
                    {
                        IsContactInfoOpen = true;
                    }
                    else
                    {
                        IsContactInfoOpen = false;
                    }
                }));
            }
        }
        private RelayCommands _closeContactInfo;

        public RelayCommands CloseContactInfo
        {
            get
            {
                return _closeContactInfo ?? (_closeContactInfo = new RelayCommands(async obj =>
                {
                    IsContactInfoOpen = false;
                }));
            }
        }

        private RelayCommands _openSearchCommand;

        public RelayCommands OpenSearchCommand
        {
            get
            {
                return _openSearchCommand ?? (_openSearchCommand = new RelayCommands(async obj =>
                {
                    if (IsSearchBoxOpen == false)
                    {
                        IsSearchBoxOpen = true;
                    }
                    else
                    {
                        IsSearchBoxOpen = false;
                    }
                }));
            }
        }
        private RelayCommands _clearSearchCommand;

        public RelayCommands ClearSearchCommand
        {
            get
            {
                return _clearSearchCommand ?? (_clearSearchCommand = new RelayCommands(async obj =>
                {
                    if (!string.IsNullOrEmpty(TxtEntered))
                    {
                        TxtEntered = "";
                    }
                    else
                    {
                        IsSearchBoxOpen = false;
                    }
                }));
            }
        }

        private RelayCommands _openMessageSearchCommand;

        public RelayCommands OpenMessageSearchCommand
        {
            get
            {
                return _openMessageSearchCommand ?? (_openMessageSearchCommand = new RelayCommands(async obj =>
                {
                    if (IsMessageSearchBoxOpen == false)
                    {
                        IsMessageSearchBoxOpen = true;
                    }
                    else
                    {
                        IsMessageSearchBoxOpen = false;
                    }
                }));
            }
        }
        private RelayCommands _clearMessageSearchCommand;

        public RelayCommands ClearMessageSearchCommand
        {
            get
            {
                return _clearMessageSearchCommand ?? (_clearMessageSearchCommand = new RelayCommands(async obj =>
                {
                    if (!string.IsNullOrEmpty(TxtEntered))
                    {
                        SearchMessageText = "";
                    }
                    else
                    {
                        IsMessageSearchBoxOpen = false;
                    }
                }));
            }
        }

        private RelayCommands _PinChatCommand;

        public RelayCommands PinChatCommand
        {
            get
            {
                return _PinChatCommand ?? (_PinChatCommand = new RelayCommands(async obj =>
                {
                    if (obj is ConservationsMy v)
                    {
                        if (!FilteredPinnedConservations.Contains(v))
                        {
                            await FindParticipant(v);
                            await UpdateParticipant(true, false);
                            ConservationsMies.Remove(v);
                            FilteredConservations.Remove(v);
                            PinnedConservations.Add(v);
                            FilteredPinnedConservations.Add(v);
                            v.Is_Pinned = true;
                            //v.ChatIsPinned = true;
                            if (ArchivedConservations != null)
                            {
                                if (ArchivedConservations.Contains(v))
                                {
                                    ArchivedConservations.Remove(v);
                                    v.Is_Archived = false;
                                }
                            }
                        }
                        // OnPropertyChanged("PinnedChats");
                        //OnPropertyChanged("FilteredPinnedChats");
                        // OnPropertyChanged("UsersList");
                        // OnPropertyChanged("FilteredChats");
                    }
                }));
            }
        }
        private RelayCommands _UnPinChatCommand;

        public RelayCommands UnPinChatCommand
        {
            get
            {
                return _UnPinChatCommand ?? (_UnPinChatCommand = new RelayCommands(async obj =>
                {

                    if (obj is ConservationsMy v)
                    {
                        if (!FilteredConservations.Contains(v))
                        {
                            await FindParticipant(v);
                            await UpdateParticipant(false, false);
                            ConservationsMies.Add(v);
                            FilteredConservations.Add(v);
                            PinnedConservations.Remove(v);
                            FilteredPinnedConservations.Remove(v);
                            v.Is_Pinned = false;
                            //v.IsPinned = false;

                        }
                        // OnPropertyChanged("PinnedChats");
                        //OnPropertyChanged("FilteredPinnedChats");
                        // OnPropertyChanged("UsersList");
                        // OnPropertyChanged("FilteredChats");
                    }
                }));
            }
        }

        private RelayCommands _archiveChatCommand;
        public RelayCommands ArchiveChatCommand
        {
            get
            {
                return _archiveChatCommand ?? (_archiveChatCommand = new RelayCommands(async obj =>
                {
                    if (obj is ConservationsMy v)
                    {
                        if (!ArchivedConservations.Contains(v))
                        {
                            await FindParticipant(v);
                            await UpdateParticipant(true, true);
                            ArchivedConservations.Add(v);
                            ConservationsMies.Remove(v);
                            FilteredConservations.Remove(v);
                            PinnedConservations.Remove(v);
                            FilteredPinnedConservations.Remove(v);
                            v.Is_Archived = true;
                            v.Is_Pinned = false;

                        }
                    }
                }));
            }
        }
        private RelayCommands _unarchiveChatCommand;
        public RelayCommands UnArchiveChatCommand
        {
            get
            {
                return _unarchiveChatCommand ?? (_unarchiveChatCommand = new RelayCommands(async obj =>
                {
                    if (obj is ConservationsMy v)
                    {
                        if (!FilteredConservations.Contains(v))
                        {
                            await FindParticipant(v);
                            await UpdateParticipant(false, true);
                            ConservationsMies.Add(v);
                            FilteredConservations.Add(v);
                        }
                        ArchivedConservations.Remove(v);
                        v.Is_Archived = false;
                        v.Is_Pinned = false;
                    }
                }));
            }
        }
        private RelayCommands _replyCommand;

        public RelayCommands ReplyCommand
        {
            get
            {
                return _replyCommand ?? (_replyCommand = new RelayCommands(async obj =>
                {
                    if (obj is MessagesListMy v)
                    {
                        MessageToReplyText = v.Message;
                        //MessageToReplyText=v.ReceivedMessage
                    }
                    else
                    {
                        //MessageToReplyText=v.SentMessage

                    }

                    //Показ MessageBox когда мы нажимаем кнопку переслать
                    FocusMessageBox = true;

                    //Сделать это сообщение пересланным
                    IsThisReplyMessage = true;
                }));
            }
        }
        private RelayCommands _cancelReplyCommand;

        public RelayCommands CancelReplyCommand
        {
            get
            {
                return _cancelReplyCommand ?? (_cancelReplyCommand = new RelayCommands(async obj =>
                {
                    IsThisReplyMessage = false;
                    MessageToReplyText = string.Empty;
                }));
            }
        }
        private RelayCommands _sendMessageCommand;

        public RelayCommands SendMessageCommand
        {
            get
            {
                return _sendMessageCommand ?? (_sendMessageCommand = new RelayCommands(async obj =>
                {
                    SendMessage();
                }));
            }
        }
        string filename;

        private RelayCommands _openFileDialogCommand;

        public RelayCommands OpenFileDialogCommand
        {
            get
            {
                return _openFileDialogCommand ?? (_openFileDialogCommand = new RelayCommands(async obj =>
                {
                    OpenFileDialog dlg = new OpenFileDialog()
                    {
                        Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.docx;*.doc;*.pdf;*.xlsx;*.xls"
                    };
                    if (dlg.ShowDialog()==false)
                    {
                        return;
                    }
                    filename = dlg.FileName;
                    string ext = System.IO.Path.GetExtension(filename);
                   
                    //string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name; //полностью
                    string user = Environment.UserName; //просто username
                    if (ext == ".docx" || ext == ".xlsx" || ext == ".xls" || ext == ".doc" || ext == ".pdf")
                    {
                        int count = MessagesListNow.Count;
                        MessagesListMy cons = new MessagesListMy
                        {
                            Id = ++count,
                            ImagePho = null,
                            Message = null,
                            Created_at = DateTime.Now,
                            Deleted_at = null,
                            Sender_id = UserInfoConstant.ID,
                            Type_id = null,
                            IsSender = true,
                            FileURL = dlg.FileName,
                            Razgovor_id = SelectedCons.Id
                        };

                        MessagesListNow.Add(cons);
                        MessagesListMy.Add(cons);
                        MessageText = string.Empty;
                        IsThisReplyMessage = false;
                        MessageToReplyText = string.Empty;
                        await Task.Run(async () =>
                        {
                            try
                            {
                                var api = new DiskHttpApi("y0_AgAAAAA3aTfeAAu9YAAAAAEEDhgkAABRlpaPcV9Ao73LN40z_PIzx5qjOQ");
                                var roodFolderData = await api.MetaInfo.GetInfoAsync(new YandexDisk.Client.Protocol.ResourceRequest
                                {
                                    Path = "/"
                                });
                                var link = await api.Files.GetUploadLinkAsync("/" + System.IO.Path.GetFileName(filename), overwrite: false);
                                using (var fs = File.OpenRead(filename))
                                {
                                    await api.Files.UploadAsync(link, fs);
                                }
                                var testFolderData = await api.MetaInfo.GetInfoAsync(new YandexDisk.Client.Protocol.ResourceRequest
                                {
                                    Path = "/"
                                });
                                foreach (var item in testFolderData.Embedded.Items)
                                {
                                    //bibaoa.Add(item.Name + " " + item.Type);

                                }

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        });
                        PostMessageAsync(cons);
                    }
                    else if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
                    {
                        int count = MessagesListNow.Count;
                        MessagesListMy cons = new MessagesListMy
                        {
                            Id = ++count,
                            ImagePho = File.ReadAllBytes(dlg.FileName),
                            Message = null,
                            Created_at = DateTime.Now,
                            Deleted_at = null,
                            Sender_id = UserInfoConstant.ID,
                            Type_id = null,
                            IsSender = true,
                            FileURL = null,
                            Razgovor_id=SelectedCons.Id
                        };
                        MessagesListNow.Add(cons);
                        MessagesListMy.Add(cons);
                        MessageText = string.Empty;
                        IsThisReplyMessage = false;
                        MessageToReplyText = string.Empty;
                        PostMessageAsync(cons);
                    }
                }));
            }
        }
        private RelayCommands _chooseDocumentCommand;

        public RelayCommands ChooseDocumentCommand
        {
            get
            {
                return _chooseDocumentCommand ?? (_chooseDocumentCommand = new RelayCommands(async obj =>
                {
                    if (obj is MessagesListMy boba)
                    {
                        string path = string.Format($"c:\\users\\{Environment.UserName}\\Downloads");
                        var desDir = System.IO.Path.Combine(path, "MessengerData");
                        if (!Directory.Exists(desDir))
                        {
                            Directory.CreateDirectory(desDir);
                        }
                        string pathfile = System.IO.Path.GetFileName(boba.FileURL);
                        if (!File.Exists(desDir + "\\" + pathfile))
                        {
                            await Task.Run(async () =>
                            {
                                try
                                {
                                    var api = new DiskHttpApi("y0_AgAAAAA3aTfeAAu9YAAAAAEEDhgkAABRlpaPcV9Ao73LN40z_PIzx5qjOQ");

                                    var testFolderData = await api.MetaInfo.GetInfoAsync(new YandexDisk.Client.Protocol.ResourceRequest
                                    {
                                        Path = "/"
                                    });
                                    await api.Files.DownloadFileAsync(path: testFolderData.Path + pathfile, System.IO.Path.Combine(desDir, pathfile));
                                    //  var lnk = await api.Files.DownloadFileAsync(path: testFolderData.Path + System.IO.Path.GetFileName(boba.FileURL));
                                    //foreach (var item in testFolderData.Embedded.Items)
                                    //{
                                    //    await api.Files.DownloadFileAsync(path: boba.FileURL, Path.Combine(desDir, ));
                                    //}
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            });
                        }
                        else
                        {
                            var psi = new ProcessStartInfo(desDir + "\\" + pathfile)
                            {
                                UseShellExecute = true
                            };

                            Process.Start(psi);
                        }
                    }
                    
                }));
            }
        }
        #endregion Commands

    }
}
