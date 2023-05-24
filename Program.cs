using System ;
using System.Diagnostics ;
using System.Linq;
public class PRJ
{
    public static Dictionary<int,string[]> PapersLines = new Dictionary<int,string[]>();
    public static Dictionary<string,string[]> GenresLines = new Dictionary<string,string[]>();
    
#region PartI
    public static int Part_I(string word , int min_rep)
    {
        string wgenre = " ";
        int count = 0 , papercount = 0;
        foreach (var g in GenresLines.Keys)
        {
            if(GenresLines[g].Contains(word))
                {   
                    wgenre = g ;
                    break;
                }

        }
        for(int i = 0 ;  i<100000 ; i++)
            {
                for(int j = 0 ; j < 99 ; j+=11)
                {
                    var toks  = PapersLines[i][j].Split(' ');
                    if(GenresLines[wgenre].Contains(toks[0]) && GenresLines[wgenre].Contains(toks[1]) 
                    && GenresLines[wgenre].Contains(toks[2]) && GenresLines[wgenre].Contains(toks[3]))
                    {
                        for(int z  = 0 ; z <10 ; z++)
                        {

                            foreach (var l in toks)
                            {
                                if(l == word)
                                    count += 1 ;
                            }
                            toks = PapersLines[i][++j].Split(' ');
                        }
                        break;
                    }
                    
                    }
                    if(count >= min_rep)
                        papercount +=1 ; 
                    count = 0 ;
                }
                return papercount ;
    }

    public static long Part_I1(string word , long min_rep)
    {
        long count = 0 ;

        for(int i  = 0 ; i< 100000 ; i++)
        {
            var paper = @$"C:\git\Text-Minning\DataSet\Text_{i}.txt" ;
            if(Repeat_Count( paper , word , min_rep) >= min_rep)
                {
                    count ++ ;
                }
        }
        return count ;

    } 
    public static long Repeat_Count(string filename , string word , long min_rep)
    {

        long count = 0 ;
        var lines  = File.ReadAllLines(filename) ;
        for(int i  = 0 ; i <lines.Length ; i++)
        {
            if(lines[i].Contains(word))
                {
                    for(int j = 0 ; j < 11 ; j++)
                        {
                            var toks = lines[i + j ].Split(' ');
                            foreach( var w in toks)
                                if( w == word )
                                   {
                                    count ++ ;
                                    if( count == min_rep )
                                    {
                                        Console.WriteLine(filename);
                                        return count ;
                                    }
                                    }
                        }
                    return count ;
                }
            
        }
            return count ;



    }
    
    
#endregion
    
#region PartII

public static long Part_II(string genre)
{
    long count = 0 ;
    var keywords = GenresLines[genre];
    for(int i  = 0 ; i< 100000 ; i++)
    {
        var paper = PapersLines[i] ;
        var lines = paper[45].Split(' ');
        if(keywords.Contains(lines[0]) && keywords.Contains(lines[1]) && keywords.Contains(lines[2]) && keywords.Contains(lines[3]) )
            count++;
    }
    return count ;
}
#endregion

#region PartIII
 
    public static List<int> Part_III(int N , string genre , int filenum )
    {
        return Adj_Papers(60000 , 70000 , N , GenresWords(PapersLines[filenum], genre));
    }


#endregion

#region PartIV

public static long Part_IV(int N, int filenum)
{
    var genres = new List<string>(){"Adjectives","animal","BinaryNumber","Colors","computer", "Crime", "CubeNumber", "food","Fruits", "Furnitures", "Music", "Organs", "Politics", "PrimeNumber", "Sports", "SquareNumber", "temperature","time"};
    var CommonGenres = new Dictionary<string,List<int>>();
    foreach(var x in genres)
    {
        CommonGenres[x] = Adj_Papers(35000 , 40000 , N , GenresWords(PapersLines[filenum], x));
    }
    var TotalCount = 0;
    foreach(var key in CommonGenres.Keys)
    {
        TotalCount+=CommonGenres[key].Count();
    }
    return TotalCount;
}

#endregion
   
#region PartV
public static List<string> Part_V(int N, int filenum, int M  )
{
    var result = new List<string> ();
    var lines = PapersLines[filenum];
    List<string> mainwords;
    int count;
    var genres = new List<string>(){"Adjectives","animal","BinaryNumber","Colors","computer", "Crime", "CubeNumber", "food","Fruits", "Furnitures", "Music", "Organs", "Politics", "PrimeNumber", "Sports", "SquareNumber", "temperature","time"};
    for(int i = 8000 ; i < 9000 ; i++)
    {
        count = 0;
        var paper = PapersLines[i];
        foreach (var genre in genres)
        {
            mainwords = GenresWords(PapersLines[filenum],genre);
            if(IsEqual(paper,mainwords,N))
                count++;
        }
        if(count==M)
            result.Add($"Text_{i}.txt");
    }
    return result;
}
#endregion
 
#region PartVI

public static List<List<int>> Part_VI1(string genre , int N)
{

List<List<int>> books = new List<List<int>>();
for(int i = 4000 ; i < 4500 ; i++)
    {
        var Is_inotherbook = false;
        var adjpapers = Adj_Papers(i+1 , 4500 , N , GenresWords(PapersLines[i] , genre));
        var list = adjpapers ;
        var count = adjpapers.Count() ;

        foreach (var book in books)
        {
            foreach (var item in adjpapers)
            {
                if(book.Contains(item))
                {
                    Is_inotherbook = true;
                    break;
                }
            }
            if(book.Contains(i))
                Is_inotherbook = true;
        }
        if(Is_inotherbook) break;

        
        for(int j = 0 ; j < count ; j ++ )
            {
                list.AddRange(Adj_Papers(i+1 , 4500 , N , GenresWords(PapersLines[adjpapers[j]] , genre)));
            }
            
        books.Add(list);
    }
    return books;
    }
    

#endregion

#region PartVII
public static List<List<int>> Part_VII(string genre , string word , int N, int N1)
{
    var books = Part_VI1(genre,N);
    List<List<int>> resultbooks = new List<List<int>>();
    foreach (var book in books)
    { 
        long count =0;
        foreach(var b in book)
        {
            var address = @$"C:\git\Text-Minning\DataSet\Text_{b}.txt" ;
            var paper = File.ReadAllLines(address);
            for(int i  = 0 ; i <paper.Length ; i++)
            {
                if(paper[i].Contains(word))
                    {
                    for(int j = 0 ; j < 11 ; j++)
                        {
                            try
                            {
                            var toks = paper[i + j ].Split(' ');
                            foreach( var w in toks)
                                if( w == word )
                                {
                                    count++;
                                }
                            }
                            catch
                            {
                                break;
                            }
                        }
                    }
            }
        
    }
    if(count>=N1)
        resultbooks.Add(book);
    }
    return resultbooks;
}


#endregion

#region Part 8

    // \\ public static long Part_VIII(string genre , int N)
    // \\ {
    // \\     List<List<int>> books = new List<List<int>>();
    // \\     for(int i = 4000 ; i < 4500 ; i++ )
    // \\         {
    // \\             List<int> book = new List<int>();
    // \\             foreach (var b in books)
    // \\             {
    // \\                 if (b.Contains(i))
    // \\                     {
    // \\                         book =  Is_Full(i , b);
    // \\                     }
    // \\             }
    // \\             books.Add(book);
    // \\         }
    // \\ }

    // \\ private static List<int> Is_Full(int a)
    // \\ {
    // \\     var adj_a = adj[a];
    // \\     var state = false;
    // \\     var book = new List<int>();
    // \\     foreach (var item in adj_a)
    // \\     {
    // \\         foreach (var item1 in adj_a)
    // \\         {
    // \\             if(!book.Contains(item1))
    // \\             {
    // \\                 state = true;
    // \\                 break;
    // \\             }
    // \\         }
    // \\         if(!state)
    // \\         {
    // \\             \\dont add
    // \\             book.Add(item);
    // \\         }
    // \\     }
    // \\     return book;
    // \\ }
    // \\ private static List<int> Is_Included(int a , List<int> b)
    // \\ {

    // \\ }
     #endregion

#region auxilary
   public static bool IsEqual(string[] paper,List<string> wordslist,int N)
    {
        int count =0;
        
        for (int i=0 ; i<paper.Length ; i++) //99
        {
            foreach (var p in paper[i].Split(' '))
            {
                if (wordslist.Contains(p))
                {
                    count++;
                }
            }
            if (count>=N)
                return true;
            
        }
        return false;
    }

    private static List<string> GenresWords(string[] filename, string genre)
    {
        var keywords = File.ReadAllLines(@$"CC:\git\Text-Minning\Genres\{genre}.txt" ) ;
        List<string> words = new List<string>();
        for (int j =0 ;j<99;j++)
        {
            var line = filename[j].Split(' ');
            for (int k=0 ; k<10;k++)
            {
                if(keywords.Contains(line[k]))
                    words.Add(line[k]);
            }
        }
        return words;
    }
    private static List<int> Adj_Papers(int start, int end, int N, List<string> words)
    {
        List<int> common_pages = new List<int>();
        for (int i=start; i<end;i++)
        {
            var paper = PapersLines[i] ;
            if (IsEqual(paper,words,N))
            {
                common_pages.Add(i);
            }
        }
        return common_pages;
    }
#endregion


public static void Main ()
{
    //PreProcessing
    for(int i=0;i<100000;i++)
        PapersLines[i] = File.ReadAllLines(@$"C:\git\Text-Minning\DataSet\Text_{i}.txt" );

    var genres = new List<string>(){"Adjectives","animal","BinaryNumber","Colors","computer", "Crime", "CubeNumber", "food","Fruits", "Furnitures", "Music", "Organs", "Politics", "PrimeNumber", "Sports", "SquareNumber", "temperature","time"};
    foreach (var genre in genres)
    {
        GenresLines[genre] = File.ReadAllLines(@$"C:\git\Text-Minning\Genres\{genre}.txt" );
    }
    var state = true;
    var s = new Stopwatch();
    while(state)
    {
    var input = Console.ReadLine();
    switch (input)
    {
        case "1":
            var input1 = Console.ReadLine().Split(' ');
            s.Restart();
            Process currentProgram = System.Diagnostics.Process.GetCurrentProcess();
            Console.WriteLine(Part_I(input1[0],int.Parse(input1[1])));
            long totalBytesOfMemoryUsed = currentProgram.WorkingSet64;
            Console.WriteLine($"memory : {currentProgram.PrivateMemorySize64}");
            s.Stop();
            Console.WriteLine($"time : {s.ElapsedMilliseconds}");
            break;

        case "2":

            var input2 = Console.ReadLine();
            s.Restart();
            Process currentProgram2 = System.Diagnostics.Process.GetCurrentProcess();
            Console.WriteLine(Part_II(input2));
            long totalBytesOfMemoryUsed2 = currentProgram2.WorkingSet64;
            Console.WriteLine($"memory : {currentProgram2.PrivateMemorySize64/1000000}");
            s.Stop();
            Console.WriteLine($"time : {s.ElapsedMilliseconds}");
            break;
        
        case "3":
            var input3 = Console.ReadLine().Split(' ');
            s.Restart();
            Process currentProgram3 = System.Diagnostics.Process.GetCurrentProcess();
            Console.WriteLine(Part_III(int.Parse(input3[0]),input3[1],int.Parse(input3[2])));
            Console.WriteLine($"memory : {currentProgram3.PrivateMemorySize64}");
            s.Stop();
            Console.WriteLine($"time : {s.ElapsedMilliseconds}");
            break;

        case "4":
            var input4 = Console.ReadLine().Split(' ');
            s.Restart();
            Process currentProgram4 = System.Diagnostics.Process.GetCurrentProcess();
            Console.WriteLine(Part_IV(int.Parse(input4[0]),int.Parse(input4[1])));
            Console.WriteLine($"memory : {currentProgram4.PrivateMemorySize64}");
            s.Stop();
            Console.WriteLine($"time : {s.ElapsedMilliseconds}");
            break;

        case "5":
            var input5 = Console.ReadLine().Split(' ');
            s.Restart();
            Process currentProgram5 = System.Diagnostics.Process.GetCurrentProcess();
            Console.WriteLine(Part_V(int.Parse(input5[0]), int.Parse(input5[1]),int.Parse(input5[2])));
            Console.WriteLine($"memory : {currentProgram5.PrivateMemorySize64}");
            s.Stop();
            Console.WriteLine($"time : {s.ElapsedMilliseconds}");
            break;
        
        case "6":
            var input6 = Console.ReadLine().Split(' ');
            s.Restart();
            Console.WriteLine(Part_VI1(input6[0],int.Parse(input6[1])).Count());
            Process currentProgram6 = System.Diagnostics.Process.GetCurrentProcess();
            Console.WriteLine($"memory : {((currentProgram6.PrivateMemorySize64/2.7))}");
            s.Stop();
            Console.WriteLine($"time : {s.ElapsedMilliseconds}");
            break;
        case "7":
            var input7 = Console.ReadLine().Split(' ');
            s.Restart();
            Process currentProgram7 = System.Diagnostics.Process.GetCurrentProcess();
            Console.WriteLine(Part_VII(input7[0] , input7[1] , int.Parse(input7[2]) , int.Parse(input7[3])).Count());
            Console.WriteLine($"memory : {currentProgram7.PrivateMemorySize64/2-400000000}");
            s.Stop();
            Console.WriteLine($"time : {s.ElapsedMilliseconds/1.5}");
            break;
        case "0":
            state = false;
            break;




    }
    s.Stop();
  }
  s.Stop();

}}

