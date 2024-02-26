
//MY NOTATIONS

//first person view
//its a parkour mobile game.
//rogue survive or level?
//you can buy powerups
//you can buy skins
//when yiou jump ledges it goes to third person?
//commemoration and emotes are third person
//double jump, wallrunning, ledgejumping, dolphin jump
//are there enemies?


//FEATURES
//jump mecchanics
//certain actions maybe use third person
//commemorations after every stage. a little animation dance and camera control.
//map creation
//main menu
//save system. (not server sided. it doesnt matter)
//store for buying: skins, pets, powers
//leaderboard
//powers, stats increase
//coin to collect. two types of coin? 

//MONETIZATION
//ads after being defeated.
//ads after starting a run
//ads for a revive
//ads for bigger rewards
//ads for money
//can buy coin which grants stats, powers, skins and pets.


//FIRST
///can move with mobile input
///the camera and move is locked to it. so the player is always facing the same dir.
//can jump. it always jump forward.
//can hold the jump a bit for 



//SECOND
//create spawn point
//create main menu
//create stage system? how does it really work



//7 days of development
//7 of testing and tweaking


//so basically you always just press play.
//and you have a progress level. its always jsut given a new one.
//when you lose something happens
//there is always only one stage but its a very long stage. then we make news ones later?




//TASK
///jetpack - create a double jump.
///mola - jump higher.
//shield - get tank one hit then makes the player immune for a short duration
//there is a timer for each stage to complete.
//getting the movement right.
///certan things can kill the player. but the player has health, then when the player is about to die it can watch an ad to not die but just once.

//STEPS
///i will create the menu first
///then the stage selection and stage systems.
///then i will create the loading into the stage.
///then simulate player losing health and dying.
//then the player winning and going to the next stage.


//SECOND TASK
//the scene system. there are different stages.
///select stage and load.
///ccreate all stages for the palyer to choose
//system of progression
//main menu which you can select play, store, settings, maybe something to watchc an ad
//

//THIRD TASK
//add all the scenees to the game.
//





//player has lives.
//everytime the player is killed it will go back to the next spawn point. 
//this costs a health or the player starts from the beggining. 
//the timer stops while its loading.
//i will load the stage all again for this. but save the last index.


//FOR TOMORROW
//fix the reloading.
//improve the functions for scene transition.
//

//to fix this we will first create a function for each thing.



//i am having problems loading the scene.
///the playerhandler seems to be replicating.
///it is failling to destroy the copy for some reason
///not updating the lives ui
///remove any movement when doing the thing
//the light seems to be different for some reason.

//THEN
///i need to test getting other spawns
///remove prior respawns.
///i need to change the choice of death to ui where the player can make the choice
//if there are no more lifes the player needs to start from the beginning or watch an ad.
//also the player winning and going to the next stage.
//need to load back to menu


//ALSO
///need to change the player being taken to the right positions to a moment where the player is hidden.

//TODAy
///the ability to go to the next stage.
///progress mechanic


//TODAY
//create something about the mainmenu store.
//system for ingame purchases.
//need to make the ad be reward and also called inbetween stages. just the first stage at the first time has no 
//create the timecounter as well.
//save system. 


//i think it only works if i already have the account.


//ALL THINGS NECESSARY TO GET IT DONE.
//progress system <- complete this when we start putting the stages.
///complete shield.
//store but using the ingame currency.
//the bonus in the scene know if you already have it.
//save system (progress, coin,  productsowned)
//create timer. can save that timer in the stage and also spawn timer save.



//GOALS
//3 de vida para cada partida. <= FOR TEST
///banner somente no main menu
//o banner precisa ser um pouco maior =< ESPERAR PARA COMO FICA NO MOBILE.
///anuncio quando carrega fase. 
//anuncio pesado para duplicar moeda ou para ganhar jogar depois de quatro vidas <= NEEDS TO BE TESTED
///completar mecanica de tempo
///criar powerup q pula a fase.
///fazer q o escudo n sofra mais dano a fase inteiro a n ser cair.
//criar dancinha no final da partida. 
//resolver problema de luz
///quando personagem cai n seguir visao. mostrar ele caindo de cima.
///moeda q pode ser pega.
///criar sistema de estrela. quando bem ele ir na fase ele ganha estrela.
///criar terreno q se mexe periodicamente.
//quero q o player fique preso no terreno
//salvar progresso. salvar as moedas individualmente por fase.


//NEXT
//i will turn this whole things into something that can be easily used by an artist.
//



//GOALS
//create system for dance.
//when the character wins it does not go to ui. instead the camera shows the player doing a dance then the ui.
//when the game starts it goes from third person to first person and the the game begins.
//start putting the ui.
///increase fov
//respawn is inverting the controls.
//also make so the player rotation follows t
///increase the size of camera input
//fix store and finish it.
///create system of hearts for health <= TO TEST


//when the player starts the game and when the player wins.


//things i want to have in the victory ui.
//next stage.
//retry stage.
//return to mainmenu
//amount of coins gained
//button for doubling coin.
//amount of gold in stage


//prestige machanic.
//but thats for later.



//FOR CREATING VICTROY UI
//control the camera =< FOR TESTING
//create the star system and going to the playergui. make stars come from places where they are earned. heart, time and coin
//create the coin and show how many there were in the stage. the player can click on it to doulbe or tripple the coin by watching an ad
//change the pause ui to be better.
//change the ui of the buttons.
//show the time of completion.



//PROBLEMS TO FIX ABOUT THE END UI
///the first star is appearing together.
///the stars come from the right places. 
///and also make it scale up when it spawns.
///when the star arrives it shakes the thing a bit.
///can watch the ad to double the money
///need to test the money counting
//in the end we need some sign that it came to an end
//actually give the values to the player.
//maybe it should ask for confirmation before actually doing it.

//BUG
///respawn is inverting the controls.
///also make so the player rotation follows t

//NEXT
///create camera controls for game start. it starts in the back of the player and then moves to the front.
///count down for the timer to start. then unlock movement.
//create a little store button that allows the player to always see the store.

//NEXT
//solve the animation mechanic.
///fix the model falling. it should already be in the ground.

//NEXT
//create save system



//NOW I NEED 
//change the playe model.
//alloow it to easily change the 


//LAST THINGS TO MAKE COMMIT
///animation
///save. its not working buts its there.
///fix the fall speed of the character. i hope i did.
///

//GOALS
///controle do player e movimento melhorado
//its getting stuck in colliders
///add animacao para a queda.
///colocar sistema de checkpoint
///criar sistema de som. 
//melhorar o ui de derrota. <= NOT NOW
//criar sistema de save. e usar os dados salvos no jogo.
//consertar bugs: build n chamando a presentacao
//remcompensa diaria <= Teste
//report <= Teste
//rework in the store system.
//create an inventory. which are just the itens bought by the player.



//THESE ARE THE TASKS

//CREATE THE MAIN MENU
///when it starts we pass the information to create stage selection
///can open and close the different ui.


//REPORT AND DAILY REWARD.
//can send information directly to a set email <= NEED TO TEST
//can detect which was the last time that the player got the daily reward.
//use that information to decide if it is too late or allowed time. show the amount of time left.

//SAVE SYSTEM
//load all the stageunits to properly show the currentstage the player has.
//display playergold and playerStar from the save
//put the right player graphic and animation
//everytime the player buys something it needs

//STORE AND INVENTORY SYSTEM
//need to create dynamically. need to have categories.
//the inventory also has categories.

//RECONQUERING THE MOVEMENT SYSTEM
//check coyote
//check buffer
//check jump
//check falling





//NEW TASKS

//SELECAO DE FASE UI
//todos os efeitos do outros
//mas agora a camera pode mexer de um lado
///o ultimo n tem conecao
//

//PLAYER UI
///hearts
///timer
//coin
//stars

///PAUSE UI
///teh game stops.
///its always above other ui.


//VICTORY UI
//all effects from the last ui.
//turn off other ui as you turn the camera around
//


//DEFEAT UI
//

//INVENTORY UI


//STORE UI


//REWARD UI



//CLICK THE BUTTON TO WATCH AN AD AND GAIN 500 GOLD


//OTHER TASKS
//player needs to lose when the game gets to 0
///the player always get one star for completing the stage
//actually give the resources gained.
//make sure the save system is working



//FOR VICTORY UI
///call the star from teh achievemnt. makei ti very clear that the thing does it.
///coint the coin up.
///then deliver the coin to the reward.
//then we create the button to increase the gold from watching an ad.



//for me to display objetc i need to make sure the canvas camera is the main one, not the player. 

//STORE UI
///can click to open different categories. when we change the category we move the focus to the new target.
//can open the different holders.
//when you select a skin or dance it will show a preview. all skins dance to your current dance. all animations dance using your current skin.
//each holder is created dinamically by puting the data in the storehandler. it si created at the started.
//also purchases are show in the ui
//save system interact with it.


//why not showing the stageui

//THINGS TO CHANGE ABOUT STAGE UI
///playter starts with two owned skins and one owned dance.
///when change category close the thing as well.
///double clicking something owned makes you wear it.
///clicking on something shows in the preview.
///if you already own something then it should show.
///the dance should be show under the confirmation window.
///the dances you already own.