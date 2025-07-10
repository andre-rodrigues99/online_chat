using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text;

namespace Models.Messages;

public class ClientMessage
{
    public int msg_to { get; set; }
    public string? content { get; set; }
    public long timestamp { get; set; }

    public ClientMessage FromMessage(Message msg)
    {
        ClientMessage n_obj = new ClientMessage
        {
            msg_to = msg.msg_to,
            content = msg.content,
            timestamp = msg.timestamp
        };
        
        return n_obj;
    }
}

public class Message
{
    public int id { get; set; }
    public int session_id { get; set; }
    public string? content { get; set; }
    public int msg_to { get; set; }
    public long timestamp { get; set; }


    public Message OpenMessage(int id, int session_id, string msg_content)
    {
        ClientMessage? msg = JsonSerializer.Deserialize<ClientMessage>(msg_content);

        if (msg == null)
        {
            throw new InvalidOperationException("Failed to deserialize message content.");
        }

        this.id = id;
        this.session_id = session_id;
        this.content = Sanitize(msg.content);
        this.msg_to = msg.msg_to;
        this.timestamp = msg.timestamp;

        return this;
    }

    
    private string Sanitize(string? msg)
    {
        if (msg == null)
        {
            throw new ArgumentNullException(nameof(msg), "O conteúdo da mensagem não pode ser nulo.");
        }

        string pattern_bad_words = @"\b(bosta|merda|caralho|porra|puta|puto|fdp|filho da puta|babaca|idiota|imbecil|corno|vagabunda|vagabundo|desgraçado|desgraçada|cu|cuzão|cuzona|cacete|arrombado|arrombada|escroto|escrota|retardado|retardada|nojento|nojenta|piranha|viado|veado|boiola|baitola|canalha|safado|safada|trouxa|otário|otária|panaca)\b";
        string pattern_injection = @"['""\\;%\-‐–—]|(--|\bOR\b\s+\d+=\d+|\bAND\b\s+\d+=\d+)";
        string pattern_suspect = @"xp_cmdshell|\bOR\b\s+\d+=\d+|\b(SELECT|UNION|INSERT|UPDATE|DELETE|DROP|ALTER|EXEC|CREATE|TRUNCATE|MERGE)\b|--|;|'|\*";
        string sanitized_msg = msg;

        sanitized_msg = Regex.Replace(sanitized_msg, pattern_bad_words, "*");
        sanitized_msg = Regex.Replace(sanitized_msg, pattern_injection, String.Empty);
        sanitized_msg = Regex.Replace(sanitized_msg, pattern_suspect, String.Empty);

        if (sanitized_msg.Length > 250)
        {
            sanitized_msg = sanitized_msg.Substring(0, 250);
        }

        return sanitized_msg;
    }
}