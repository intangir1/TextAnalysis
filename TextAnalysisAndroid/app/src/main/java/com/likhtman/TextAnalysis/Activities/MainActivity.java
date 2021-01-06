package com.likhtman.TextAnalysis.Activities;

import androidx.annotation.NonNull;
import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.Toolbar;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;

import android.graphics.drawable.Drawable;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import com.google.android.material.navigation.NavigationView;
import com.likhtman.TextAnalysis.Callbacks.CallbacksOpenDefaultFragment;
import com.likhtman.TextAnalysis.DataStore;
import com.likhtman.TextAnalysis.DialogMessage;
import com.likhtman.TextAnalysis.Fragments.AddWordsFragment;
import com.likhtman.TextAnalysis.Fragments.CheckTextFragment;
import com.likhtman.TextAnalysis.Fragments.SignInFragment;
import com.likhtman.TextAnalysis.Fragments.SignUpFragment;
import com.likhtman.TextAnalysis.Logics.DrawableResize;
import com.likhtman.TextAnalysis.R;
import com.likhtman.TextAnalysis.StorageDataFromServer;
import com.squareup.picasso.Picasso;

import jp.wasabeef.picasso.transformations.CropCircleTransformation;

public class MainActivity extends AppCompatActivity implements NavigationView.OnNavigationItemSelectedListener, CallbacksOpenDefaultFragment {


    DrawerLayout drawer;
    NavigationView navigationView;
    final String TAG = MainActivity.this.getClass().getSimpleName();


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);


        Toolbar toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.addDrawerListener(toggle);
        toggle.syncState();


        drawer.addDrawerListener(new DrawerLayout.DrawerListener() {
            @Override
            public void onDrawerSlide(@NonNull View drawerView, float slideOffset) {


            }

            @Override
            public void onDrawerOpened(@NonNull View drawerView) {

            }

            @Override
            public void onDrawerClosed(@NonNull View drawerView) {

            }

            @Override
            public void onDrawerStateChanged(int newState) {
                if(!StorageDataFromServer.Get_instance().isWas_updated() &&
                        (newState == DrawerLayout.STATE_DRAGGING || newState == DrawerLayout.STATE_SETTLING) &&
                        !drawer.isDrawerOpen(GravityCompat.START)) {
                    Update_menu();
                }
            }
        });


        navigationView = (NavigationView) findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);
        navigationView.setCheckedItem(R.id.itm_check_text);

        FragmentManager fragmentManager = getSupportFragmentManager();
        CheckTextFragment fragment = new CheckTextFragment();
        fragmentManager.beginTransaction().replace(R.id.frameLayout, fragment).commit();

    }


    @Override
    public void onBackPressed() {
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        if (drawer.isDrawerOpen(GravityCompat.START)) {
            drawer.closeDrawer(GravityCompat.START);
        } else {
            super.onBackPressed();
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }


    @Override
    public boolean onOptionsItemSelected(MenuItem item) {

        int id = item.getItemId();

        if (id == R.id.action_settings) {
            return true;
        } else if (id == R.id.action_faq){
            DialogMessage.Get_instance(2, this).Show_dialog();
            Log.d(TAG, "FAQ opened");
            return true;
        } else if (id == R.id.action_help){
            DialogMessage.Get_instance(9, this).Show_dialog();
            Log.d(TAG, "FAQ opened");
            return true;
        }

        return super.onOptionsItemSelected(item);
    }


    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(@NonNull MenuItem item) {

        int id = item.getItemId();

        Fragment fragment = null;
        FragmentManager fragmentManager = getSupportFragmentManager();

        if (id == R.id.itm_sign_in) {
            fragment = new SignInFragment();
        } else if (id == R.id.itm_sign_up) {
            fragment = new SignUpFragment();
        } else if (id == R.id.itm_check_text){
            fragment = new CheckTextFragment();
        } else if (id == R.id.itm_add_words){
            if(StorageDataFromServer.Get_instance().getLogin().getUserLevel() > 0){
                fragment = new AddWordsFragment();
            } else{
                DialogMessage.Get_instance(4, this).Show_dialog();
                Log.d(TAG, "Message 'no registered' opened");
                return false;
            }
        } else if (id == R.id.itm_sign_out){
            StorageDataFromServer.Get_instance().Clear_login(this);
            fragment = new CheckTextFragment();
        } else {
            Log.e(TAG, "Fragment not changed");
            return false;
        }
        fragmentManager.beginTransaction().replace(R.id.frameLayout, fragment).commit();
        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.closeDrawer(GravityCompat.START);
        Log.d(TAG, "Fragment changed");
        return true;
    }


    private void Update_nick() {
        String user_nick = StorageDataFromServer.Get_instance().getLogin().getUserNickName();
        TextView txt_user_nick = (TextView) findViewById(R.id.txt_user_name);
        txt_user_nick.setText(user_nick);
    }


    private void Hide_menu_options(Short user_level_number) {
        Menu menu = navigationView.getMenu();
        MenuItem itm_sign_in = (MenuItem) menu.findItem(R.id.itm_sign_in);
        MenuItem itm_sign_up = (MenuItem) menu.findItem(R.id.itm_sign_up);
        MenuItem itm_sign_out = (MenuItem) menu.findItem(R.id.itm_sign_out);
        if(user_level_number > 0){
            itm_sign_in.setVisible(false);
            itm_sign_up.setVisible(false);
            itm_sign_out.setVisible(true);
        } else{
            itm_sign_in.setVisible(true);
            itm_sign_up.setVisible(true);
            itm_sign_out.setVisible(false);
        }
    }


    private void Update_level() {
        String user_level;
        Short user_level_number = StorageDataFromServer.Get_instance().getLogin().getUserLevel();
        switch (user_level_number){
            case 0:
                user_level = "guest";
                break;
            case 1:
                user_level = "registrated";
                break;
            case 2:
                user_level = "vip";
                break;
            case 4:
                user_level = "admin";
                break;
            default:
                user_level = "";
                break;
        }
        TextView txt_user_level = (TextView) findViewById(R.id.txt_user_level);
        txt_user_level.setText(user_level);
        Hide_menu_options(user_level_number);
    }


    private void Update_picture() {
        Uri uri_user_avatar;
        if(StorageDataFromServer.Get_instance().getLogin().getUserLevel() == 0){
            uri_user_avatar = Uri.parse(StorageDataFromServer.Get_instance().getLogin().getUserPicture());
        } else{
            uri_user_avatar = Uri.parse(DataStore.baseUrl + StorageDataFromServer.Get_instance().getLogin().getUserPicture());
        }

        View nav_view_header = navigationView.getHeaderView(0);
        ImageView img_user_avatar = (ImageView) nav_view_header.findViewById(R.id.img_user_avatar);
        int width = img_user_avatar.getWidth();
        int height = img_user_avatar.getHeight();
        Drawable placeholder = DrawableResize.GetInstance(this).Get_resize(this.getResources().getDrawable(R.drawable.placeholder), width, height);
        Drawable error = DrawableResize.GetInstance(this).Get_resize(this.getResources().getDrawable(R.drawable.error_placeholder), width, height);


        Picasso.get()
                .load(uri_user_avatar)
                .placeholder(placeholder)
                .error(error)
                .resize(width, height)
                .transform(new CropCircleTransformation())
                .into(img_user_avatar);
    }


    private void Update_menu(){
        Update_picture();
        Update_nick();
        Update_level();
        StorageDataFromServer.Get_instance().setWas_updated(true);
        Log.d(TAG, "Menu updated");
    }

    @Override
    public void Open_default_fragment() {
        Fragment fragment = new CheckTextFragment();
        FragmentManager fragmentManager = getSupportFragmentManager();
        fragmentManager.beginTransaction().replace(R.id.frameLayout, fragment).commit();
        navigationView.getMenu().getItem(0).setChecked(true);
        drawer.closeDrawer(GravityCompat.START);
        Log.d(TAG, "Default fragment opened");
    }
}