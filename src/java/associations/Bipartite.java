package Hungarian;

import java.io.*;
import java.util.*;

class Bipartite {

public static void main(String[] args){
//INSTRUCTIONS: compile package with javac -classpath . Hungarian/Bipartite.java
//then, run with java -Xmx1024m Hungarian/Bipartite

//<INPUT FILES>
	//household file
	File hhldfile = new File ("Households1004updated.csv");
	int hhldwidth=12; //number of columns in data
	
	//person file
	File pfile = new File ("People1011Matchingrandom.txt");

	//results file
	String ResultsFileName="July_TypeChoice/Matching_results.txt"; //for head matching
//</INPUT FILES>

//<Utility function constants>
double asc[]=new double[3];
double educ[]=new double[4];
double age[]=new double[8];
double workers[]=new double[7];
double cars[]=new double[5];

//choose utility function, sizechoice=0, typechoice=1
int UtilFunc=0; 

	//sizechoice.mod
		if(UtilFunc==0){
		asc=new double[]{1.91,0,-0.737,-0.993,-1.86,-2.63};
		educ=new double[]{0,0.150,0.150,-0.657};
		age=new double[]{0,0,1.59,1.59,1.57,1.47,1.47,1.16};
		workers=new double[]{0,0.393,5.40,5.40,5.40,5.40,5.40};
		cars=new double[]{0,0,6.96,6.96,6.96}; }
		//index is APREW_norm field

	//typechoice.mod
		if(UtilFunc==1){
		asc=new double[]{1.33,0,-2.62};
		educ=new double[]{0.142,0.590,0,-0.577};
		age=new double[]{0,0,1.49,1.49,1.44,1.17,1.49,0.987};
		workers=new double[]{0,0.473,5.49,5.49,5.49,5.49,5.49};
		cars=new double[]{0,0,7.77,0,0}; }
		//index is household type
//</Utility function constants>

//Initialization
	int hhldfilelines, pfilelines, i, j;
	String temp, currentline[];
	Bipartite classcall = new Bipartite(); //run non-static methods

	hhldfilelines=(classcall.LineNo(hhldfile)-1); //subtract 1 because we have headers
	pfilelines=(classcall.LineNo(pfile)-1);
	System.out.println("Number of households: "+hhldfilelines+"\nNumber of persons: "+pfilelines+"\n");
	
	double costmatrix[][] = new double[pfilelines][hhldfilelines];
		//rows are people
		//columns are households (head of household positions)

	double households[][] = new double[hhldfilelines][hhldwidth];
		//HHNR,ZGDE,GEBAEUDE_ID,APERW,APERW_norm,KAMS_bin,APKWL_bin,HMAT,HHTP,SHSD,SHFR,SHEN



try{
BufferedReader hhldreader = new BufferedReader(new FileReader(hhldfile));
	temp=hhldreader.readLine(); //read and discard header
	for(i=0;i<hhldfilelines;i++) { 
		temp = hhldreader.readLine();
		currentline = temp.split(",");
		for(j=0;j<currentline.length;j++) {
			households[i][j] = Double.parseDouble(currentline[j]);
		} //inner for
	} //outer for
hhldreader.close();	
    } catch (Exception e) {
      System.out.println ("Mismatch exception:" + e );
    } //catch
//*****household file now an array*****


//import persons
double persons[][] = new double[pfilelines][6];
//ID,zone,age,sex,hhldsize,education
try{
BufferedReader preader = new BufferedReader(new FileReader(pfile));
	temp=preader.readLine(); //read and discard header
	for(i=0;i<pfilelines;i++) { 
		temp = preader.readLine();
		currentline = temp.split(",");
		for(j=0;j<currentline.length;j++) {
			persons[i][j] = Double.parseDouble(currentline[j]);
		} //inner for
	} //outer for
preader.close();	
    } catch (Exception e) {
      System.out.println ("Mismatch exception:" + e);
    } //catch
//*****person file now an array*****

//*****create cost matrix*****
int AscTypeIndex=0, Swiss=0, French=0;
double min=2357, max=-100;
double rowsum[] = new double[pfilelines];
for(i=0;i<pfilelines;i++){
	for(j=0;j<hhldfilelines;j++){				
		//sizechoice.mod
		if(UtilFunc==0){
			if(households[j][7] == 1){
				Swiss=1;
			} else{
				Swiss=0;
			}
			
			if(households[j][10] == 1){
				French=1;
			} else{
				French=0;
			}
			
			if(households[j][4] == 0){
				costmatrix[i][j]=-asc[(int)households[j][4]] + 20;
			} else{
				costmatrix[i][j] = -(asc[(int)households[j][4]] + age[(int)persons[i][2]] - 0.328*persons[i][3] + educ[(int)persons[i][5]] - 0.419*Swiss - 0.393*French + workers[(int)households[j][5]] + cars[(int)households[j][6]]) + 20;
			}
		}
		//end sizechoice
		
		// typechoice.mod 
		if(UtilFunc==1){
			if(households[j][8] == 1000) {
				AscTypeIndex=0;
			}
			if(households[j][8] >= 2111 && households[j][8] <= 2422) {
				AscTypeIndex=1;
			}
			if(households[j][8] >= 3110 && households[j][8] <= 3222) {
				AscTypeIndex=2;
			}
			
			if(households[j][7] == 1){
				Swiss=1;
			} else{
				Swiss=0;
			}
			
			if(households[j][10] == 1){
				French=1;
			} else{
				French=0;
			}
			
			if(AscTypeIndex == 0){
				costmatrix[i][j]=-asc[0] + 20;
			} else{
				costmatrix[i][j] = -(asc[AscTypeIndex] + age[(int)persons[i][2]] - 0.390*persons[i][3] + educ[(int)persons[i][5]] - 0.327*Swiss - 0.376*French + workers[(int)households[j][5]] + cars[(int)households[j][6]]) + 20;
			}
		}
		// ****end typechoice
		
		//**all models
		if(persons[i][2]==0){
			costmatrix[i][j]+=50; //ensure that people younger than 15 will not be selected for household head/spouse positions
		}
		//**end all models
		
		if(costmatrix[i][j]<min){ 
			min=costmatrix[i][j]; }
		if(costmatrix[i][j]>max){
			max=costmatrix[i][j]; }
	}//inner for
}//for
System.out.println("Utilities= Min: "+min+" Max: "+max);

int[] result;

HungarianAlgorithm hu =new HungarianAlgorithm(costmatrix);
result=hu.execute();

//write results to file
try{
BufferedWriter write = new BufferedWriter(new FileWriter(ResultsFileName));

for(i=0;i<result.length;i++){
	write.write(result[i]+"\n");
	write.flush();
} //for
write.close();
}catch (Exception e) { 
System.err.println ("Error in main: "+e.getMessage()); }
} //main

public int LineNo(File fileno) {
int count = 0;
	try{
		
		BufferedReader bf = new BufferedReader(new InputStreamReader(System.in));
		if (fileno.exists()){
			FileReader fr = new FileReader(fileno);
			LineNumberReader ln = new LineNumberReader(fr);
			
			while (ln.readLine() != null){
				count++;
			}
			ln.close();
			
		}
		else{
			System.out.println("ERROR in LineNo: File does not exist!");
		}
		
	} //try
	catch(IOException e){
		e.printStackTrace();
	} //catch
return count;
} //LineNo
}